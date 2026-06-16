using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Exceptions;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace ActualiteEtAbonnement.Application.Features.SAbonnerDomaine;

public sealed class SAbonnerDomaineHandler : IRequestHandler<SAbonnerDomaineCommand>
{
    private readonly IAbonnementRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public SAbonnerDomaineHandler(
        IAbonnementRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task Handle(SAbonnerDomaineCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.UtilisateurId, Permission.SAbonnerDomaine, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à s'abonner à un domaine métier.");

        if (await _repository.ExisteDejaAsync(command.UtilisateurId, command.DomaineMetier, cancellationToken))
            throw new AbonnementDejaExistantException();

        var domaine = DomaineMetier.Creer(command.DomaineMetier);
        var abonnement = Abonnement.Creer(command.UtilisateurId, domaine);

        await _repository.SauvegarderAsync(abonnement, cancellationToken);
    }
}
