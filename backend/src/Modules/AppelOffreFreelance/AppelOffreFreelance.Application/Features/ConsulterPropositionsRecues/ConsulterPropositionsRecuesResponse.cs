namespace AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;

public sealed record PropositionResume(
    Guid Id,
    Guid CandidatId,
    decimal TarifJournalier,
    int DureeJours,
    string Methodologie,
    DateTime DateSoumission);

public sealed record ConsulterPropositionsRecuesResponse(IReadOnlyList<PropositionResume> Propositions);
