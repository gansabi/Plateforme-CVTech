using FluentValidation;

namespace GestionIdentite.Application.Features.CreerCompteEntreprise;

public sealed class CreerCompteEntrepriseValidator : AbstractValidator<CreerCompteEntrepriseCommand>
{
    private const int LongueurMinimaleMotDePasse = 8;

    public CreerCompteEntrepriseValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(e => string.IsNullOrEmpty(e) || !e.Any(char.IsWhiteSpace))
            .WithMessage("L'adresse email ne peut pas contenir d'espaces.");
        RuleFor(x => x.MotDePasse).NotEmpty().MinimumLength(LongueurMinimaleMotDePasse);
        RuleFor(x => x.NomEntreprise).NotEmpty().MaximumLength(200);
    }
}
