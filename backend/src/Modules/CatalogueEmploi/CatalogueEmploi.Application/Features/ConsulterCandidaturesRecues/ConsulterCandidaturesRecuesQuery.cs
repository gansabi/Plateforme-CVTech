using MediatR;

namespace CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;

public sealed class ConsulterCandidaturesRecuesQuery : IRequest<ConsulterCandidaturesRecuesResponse>
{
    public Guid UtilisateurId { get; init; }
    public Guid AnnonceId { get; init; }
}
