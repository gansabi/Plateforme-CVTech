using FluentValidation;

namespace AppelOffreFreelance.Application.Features.ModererAppelOffre;

public sealed class ModererAppelOffreValidator : AbstractValidator<ModererAppelOffreCommand>
{
    public ModererAppelOffreValidator()
    {
        RuleFor(x => x.AdministrateurId).NotEmpty();
        RuleFor(x => x.AppelOffreId).NotEmpty();
    }
}
