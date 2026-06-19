using FluentValidation;

namespace ActualiteEtAbonnement.Application.Features.SAbonnerDomaine;

public sealed class SAbonnerDomaineValidator : AbstractValidator<SAbonnerDomaineCommand>
{
    public SAbonnerDomaineValidator()
    {
        RuleFor(x => x.UtilisateurId).NotEmpty();
        RuleFor(x => x.DomaineMetier).NotEmpty().MaximumLength(100);
    }
}
