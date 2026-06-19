namespace CatalogueEmploi.Client.Controllers;

public sealed record PostulerAnnonceRequest(
    Guid CandidatId,
    Guid AnnonceId,
    string? LettreMotivation);
