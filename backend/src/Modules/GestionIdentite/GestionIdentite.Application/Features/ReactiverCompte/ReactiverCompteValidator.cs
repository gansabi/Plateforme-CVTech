using FluentValidation;

namespace GestionIdentite.Application.Features.ReactiverCompte;

public sealed class ReactiverCompteValidator : AbstractValidator<ReactiverCompteCommand>
{
    public ReactiverCompteValidator()
    {
        RuleFor(x => x.AdministrateurId).NotEmpty();
        RuleFor(x => x.CompteId).NotEmpty();
    }
}
