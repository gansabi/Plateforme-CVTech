using FluentValidation;

namespace GestionIdentite.Application.Features.CreerCompteCandidat;

public sealed class CreerCompteCandidatValidator : AbstractValidator<CreerCompteCandidatCommand>
{
    private const int LongueurMinimaleMotDePasse = 8;

    public CreerCompteCandidatValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(e => string.IsNullOrEmpty(e) || !e.Any(char.IsWhiteSpace))
            .WithMessage("L'adresse email ne peut pas contenir d'espaces.");

        RuleFor(x => x.MotDePasse)
            .NotEmpty()
            .MinimumLength(LongueurMinimaleMotDePasse);
    }
}
