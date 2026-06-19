using MediatR;

namespace AppelOffreFreelance.Application.Features.SoumettreProposition;

public sealed class SoumettrePropositionCommand : IRequest
{
    public Guid CandidatId { get; init; }
    public Guid AppelOffreId { get; init; }
    public decimal TarifJournalier { get; init; }
    public int DureeJours { get; init; }
    public string Methodologie { get; init; } = string.Empty;
}
