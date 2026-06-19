using MediatR;

namespace CatalogueEmploi.Application.Features.SupprimerAnnonce;

public sealed class SupprimerAnnonceCommand : IRequest
{
    public Guid AdministrateurId { get; init; }
    public Guid AnnonceId { get; init; }
}
