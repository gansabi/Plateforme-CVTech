namespace AppelOffreFreelance.Client.Controllers;

public sealed record SoumettrePropositionRequest(
    Guid CandidatId,
    Guid AppelOffreId,
    decimal TarifJournalier,
    int DureeJours,
    string Methodologie);
