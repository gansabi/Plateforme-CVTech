namespace CatalogueEmploi.Application.Features.ConsulterAnnonces;

public sealed record AnnonceResume(
    Guid Id,
    string Titre,
    string TypeContrat,
    string DomaineMetier,
    DateTime DatePublication);

public sealed record ConsulterAnnoncesResponse(IReadOnlyList<AnnonceResume> Annonces);
