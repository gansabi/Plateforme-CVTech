namespace CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;

public sealed record CandidatureResume(
    Guid Id,
    Guid CandidatId,
    DateTime DatePostulation,
    string? LettreMotivation);

public sealed record ConsulterCandidaturesRecuesResponse(IReadOnlyList<CandidatureResume> Candidatures);
