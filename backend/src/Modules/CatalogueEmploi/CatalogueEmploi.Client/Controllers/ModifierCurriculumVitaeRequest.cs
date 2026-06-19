namespace CatalogueEmploi.Client.Controllers;

public sealed record ModifierCurriculumVitaeRequest(
    Guid CandidatId,
    string Titre,
    string Resume,
    IReadOnlyList<string> CompetencesPrincipales);
