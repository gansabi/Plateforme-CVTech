using MediatR;

namespace CatalogueEmploi.Application.Features.PostulerAnnonce;

public sealed class PostulerAnnonceCommand : IRequest
{
    public Guid CandidatId { get; init; }
    public Guid AnnonceId { get; init; }
    public string? LettreMotivation { get; init; }
}
