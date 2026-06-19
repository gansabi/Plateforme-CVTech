using FluentValidation;

namespace AppelOffreFreelance.Application.Features.SoumettreProposition;

public sealed class SoumettrePropositionValidator : AbstractValidator<SoumettrePropositionCommand>
{
    public SoumettrePropositionValidator()
    {
        RuleFor(x => x.CandidatId).NotEmpty();
        RuleFor(x => x.AppelOffreId).NotEmpty();
        RuleFor(x => x.TarifJournalier).GreaterThan(0);
        RuleFor(x => x.DureeJours).GreaterThan(0);
    }
}
