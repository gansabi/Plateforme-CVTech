using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Exceptions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using MediatR;

namespace CatalogueEmploi.Application.Features.ModifierCurriculumVitae;

public sealed class ModifierCurriculumVitaeHandler : IRequestHandler<ModifierCurriculumVitaeCommand>
{
    private readonly ICurriculumVitaeRepository _repository;
    private readonly IVerificateurPermission _verificateur;

    public ModifierCurriculumVitaeHandler(
        ICurriculumVitaeRepository repository,
        IVerificateurPermission verificateur)
    {
        _repository = repository;
        _verificateur = verificateur;
    }

    public async Task Handle(ModifierCurriculumVitaeCommand command, CancellationToken cancellationToken)
    {
        if (!await _verificateur.PossedePermissionAsync(
                command.CandidatId, Permission.ModifierCurriculumVitae, cancellationToken))
            throw new PermissionRefuseeException("L'utilisateur n'est pas autorisé à modifier un curriculum vitae.");

        var cv = await _repository.TrouverParCandidatIdAsync(command.CandidatId, cancellationToken)
            ?? throw new CurriculumVitaeNonTrouveException(command.CandidatId);

        cv.Modifier(command.Titre, command.Resume, command.CompetencesPrincipales);

        await _repository.MettreAJourAsync(cv, cancellationToken);
    }
}
