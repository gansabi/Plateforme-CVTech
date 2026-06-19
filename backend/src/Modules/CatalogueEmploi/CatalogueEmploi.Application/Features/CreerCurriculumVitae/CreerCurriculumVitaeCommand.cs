using MediatR;

namespace CatalogueEmploi.Application.Features.CreerCurriculumVitae;

public sealed class CreerCurriculumVitaeCommand : IRequest<CreerCurriculumVitaeResponse>
{
    public Guid CandidatId { get; init; }
    public string Titre { get; init; } = string.Empty;
    public string Resume { get; init; } = string.Empty;
    public IReadOnlyList<string> CompetencesPrincipales { get; init; } = [];
}
