using FluentValidation;

namespace GestionIdentite.Application.Features.ConnecterUtilisateur;

public sealed class ConnecterUtilisateurValidator : AbstractValidator<ConnecterUtilisateurCommand>
{
    public ConnecterUtilisateurValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.MotDePasse).NotEmpty();
    }
}
