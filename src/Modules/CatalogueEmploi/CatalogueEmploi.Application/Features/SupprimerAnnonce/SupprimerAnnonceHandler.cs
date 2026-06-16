using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Exceptions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace CatalogueEmploi.Application.Features.SupprimerAnnonce;

public sealed class SupprimerAnnonceHandler : IRequestHandler<SupprimerAnnonceCommand>
{
    private readonly IAnnonceEmploiRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public SupprimerAnnonceHandler(
        IAnnonceEmploiRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task Handle(SupprimerAnnonceCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.AdministrateurId, Permission.SupprimerAnnonce, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à supprimer une annonce.");

        var annonce = await _repository.TrouverParIdAsync(command.AnnonceId, cancellationToken)
            ?? throw new AnnonceNonTrouveeException(command.AnnonceId);

        annonce.Supprimer();

        await _repository.MettreAJourAsync(annonce, cancellationToken);
    }
}
