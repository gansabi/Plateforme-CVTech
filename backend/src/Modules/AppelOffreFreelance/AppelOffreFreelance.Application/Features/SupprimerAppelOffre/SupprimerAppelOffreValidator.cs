using FluentValidation;

namespace AppelOffreFreelance.Application.Features.SupprimerAppelOffre;

public sealed class SupprimerAppelOffreValidator : AbstractValidator<SupprimerAppelOffreCommand>
{
    public SupprimerAppelOffreValidator()
    {
        RuleFor(x => x.AdministrateurId).NotEmpty();
        RuleFor(x => x.AppelOffreId).NotEmpty();
    }
}
