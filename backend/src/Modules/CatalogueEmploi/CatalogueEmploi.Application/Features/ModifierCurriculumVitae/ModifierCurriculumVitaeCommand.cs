using MediatR;

namespace CatalogueEmploi.Application.Features.ModifierCurriculumVitae;

public sealed class ModifierCurriculumVitaeCommand : IRequest
{
    public Guid CandidatId { get; init; }
    public string Titre { get; init; } = string.Empty;
    public string Resume { get; init; } = string.Empty;
    public IReadOnlyList<string> CompetencesPrincipales { get; init; } = [];
}
