using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Exceptions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace CatalogueEmploi.Application.Features.ModererAnnonce;

public sealed class ModererAnnonceHandler : IRequestHandler<ModererAnnonceCommand>
{
    private readonly IAnnonceEmploiRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public ModererAnnonceHandler(
        IAnnonceEmploiRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task Handle(ModererAnnonceCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.AdministrateurId, Permission.ModererAnnonce, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à modérer une annonce.");

        var annonce = await _repository.TrouverParIdAsync(command.AnnonceId, cancellationToken)
            ?? throw new AnnonceNonTrouveeException(command.AnnonceId);

        annonce.Moderer();

        await _repository.MettreAJourAsync(annonce, cancellationToken);
    }
}
