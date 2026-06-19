using MediatR;

namespace CatalogueEmploi.Application.Features.ModererAnnonce;

public sealed class ModererAnnonceCommand : IRequest
{
    public Guid AdministrateurId { get; init; }
    public Guid AnnonceId { get; init; }
}
