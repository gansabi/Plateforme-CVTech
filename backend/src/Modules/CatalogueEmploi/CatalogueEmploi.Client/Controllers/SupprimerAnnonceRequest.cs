namespace CatalogueEmploi.Client.Controllers;

public sealed record SupprimerAnnonceRequest(Guid AdministrateurId, Guid AnnonceId);
