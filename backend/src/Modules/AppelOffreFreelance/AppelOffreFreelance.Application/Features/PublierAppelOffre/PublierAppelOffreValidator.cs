using FluentValidation;

namespace AppelOffreFreelance.Application.Features.PublierAppelOffre;

public sealed class PublierAppelOffreValidator : AbstractValidator<PublierAppelOffreCommand>
{
    public PublierAppelOffreValidator()
    {
        RuleFor(x => x.UtilisateurId).NotEmpty();
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.DomaineMetier).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TjmMinimum).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TjmMaximum).GreaterThanOrEqualTo(x => x.TjmMinimum);
        RuleFor(x => x.DateLimite).GreaterThan(DateTime.UtcNow);
    }
}
