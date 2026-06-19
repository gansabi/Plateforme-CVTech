namespace CatalogueEmploi.Client.Controllers;

public sealed record CreerCurriculumVitaeRequest(
    Guid CandidatId,
    string Titre,
    string Resume,
    IReadOnlyList<string> CompetencesPrincipales);
