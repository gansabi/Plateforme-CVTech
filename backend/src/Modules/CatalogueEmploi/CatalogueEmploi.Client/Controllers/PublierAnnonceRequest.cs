namespace CatalogueEmploi.Client.Controllers;

public sealed record PublierAnnonceRequest(
    Guid UtilisateurId,
    string Titre,
    string Description,
    string TypeContrat,
    string DomaineMetier);
