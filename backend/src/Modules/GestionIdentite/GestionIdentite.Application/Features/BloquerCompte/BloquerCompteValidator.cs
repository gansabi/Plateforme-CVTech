using FluentValidation;

namespace GestionIdentite.Application.Features.BloquerCompte;

public sealed class BloquerCompteValidator : AbstractValidator<BloquerCompteCommand>
{
    public BloquerCompteValidator()
    {
        RuleFor(x => x.AdministrateurId).NotEmpty();
        RuleFor(x => x.CompteId).NotEmpty();
    }
}
