namespace CatalogueEmploi.Domaine.Evenements;

public sealed record AnnoncePubliee(
    Guid AnnonceId,
    Guid EntrepriseId,
    string Titre,
    string DomaineMetier,
    DateTime DatePublication);
