using MediatR;

namespace CatalogueEmploi.Application.Features.PublierAnnonce;

public sealed class PublierAnnonceCommand : IRequest<PublierAnnonceResponse>
{
    public Guid UtilisateurId { get; init; }
    public string Titre { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string TypeContrat { get; init; } = string.Empty;
    public string DomaineMetier { get; init; } = string.Empty;
}
