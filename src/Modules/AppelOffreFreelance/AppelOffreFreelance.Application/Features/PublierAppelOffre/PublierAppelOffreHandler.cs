using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Evenements;
using AppelOffreFreelance.Domaine.ObjetsValeur;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace AppelOffreFreelance.Application.Features.PublierAppelOffre;

public sealed class PublierAppelOffreHandler
    : IRequestHandler<PublierAppelOffreCommand, PublierAppelOffreResponse>
{
    private readonly IAppelOffreRepository _repository;
    private readonly IBusEvenements _bus;
    private readonly IVerificateurPermission _verificateur;

    public PublierAppelOffreHandler(
        IAppelOffreRepository repository,
        IBusEvenements bus,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _bus = bus;
        _verificateur = verificateur;
    }

    public async Task<PublierAppelOffreResponse> Handle(
        PublierAppelOffreCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.UtilisateurId, Permission.PublierAppelOffre, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à publier un appel d'offre.");

        var domaine = DomaineMetier.Creer(command.DomaineMetier);
        var bareme = BaremeTJM.Creer(command.TjmMinimum, command.TjmMaximum);

        var appelOffre = AppelOffre.Publier(
            command.UtilisateurId, command.Titre, command.Description,
            domaine, bareme, command.DateLimite);

        await _repository.SauvegarderAsync(appelOffre, cancellationToken);

        await _bus.PublierAsync(new AppelOffrePublie(
            appelOffre.Id, appelOffre.EntrepriseId, appelOffre.Titre,
            appelOffre.DomaineMetier.Valeur, appelOffre.DatePublication), cancellationToken);

        return new PublierAppelOffreResponse(appelOffre.Id);
    }
}
