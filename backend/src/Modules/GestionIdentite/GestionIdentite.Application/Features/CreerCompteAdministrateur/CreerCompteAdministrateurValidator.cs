using FluentValidation;

namespace GestionIdentite.Application.Features.CreerCompteAdministrateur;

public sealed class CreerCompteAdministrateurValidator : AbstractValidator<CreerCompteAdministrateurCommand>
{
    private const int LongueurMinimaleMotDePasse = 8;

    public CreerCompteAdministrateurValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(e => string.IsNullOrEmpty(e) || !e.Any(char.IsWhiteSpace))
            .WithMessage("L'adresse email ne peut pas contenir d'espaces.");

        RuleFor(x => x.NomComplet)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.MotDePasse)
            .NotEmpty()
            .MinimumLength(LongueurMinimaleMotDePasse);
    }
}
