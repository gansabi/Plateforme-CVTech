using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Enums;
using CatalogueEmploi.Domaine.Evenements;
using CatalogueEmploi.Domaine.ObjetsValeur;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace CatalogueEmploi.Application.Features.PublierAnnonce;

public sealed class PublierAnnonceHandler
    : IRequestHandler<PublierAnnonceCommand, PublierAnnonceResponse>
{
    private readonly IAnnonceEmploiRepository _repository;
    private readonly IBusEvenements _bus;
    private readonly IVerificateurPermission _verificateur;

    public PublierAnnonceHandler(
        IAnnonceEmploiRepository repository,
        IBusEvenements bus,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _bus = bus;
        _verificateur = verificateur;
    }

    public async Task<PublierAnnonceResponse> Handle(
        PublierAnnonceCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.UtilisateurId, Permission.PublierAnnonce, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à publier une annonce.");

        var typeContrat = Enum.Parse<TypeContrat>(command.TypeContrat, ignoreCase: true);
        var domaine = DomaineMetier.Creer(command.DomaineMetier);

        var annonce = AnnonceEmploi.Publier(
            command.UtilisateurId, command.Titre, command.Description, typeContrat, domaine);

        await _repository.SauvegarderAsync(annonce, cancellationToken);

        await _bus.PublierAsync(new AnnoncePubliee(
            annonce.Id, annonce.EntrepriseId, annonce.Titre,
            annonce.DomaineMetier.Valeur, annonce.DatePublication), cancellationToken);

        return new PublierAnnonceResponse(annonce.Id);
    }
}
