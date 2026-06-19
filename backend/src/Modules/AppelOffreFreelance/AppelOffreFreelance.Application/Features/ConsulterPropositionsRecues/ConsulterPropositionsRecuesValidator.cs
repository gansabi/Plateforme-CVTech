using FluentValidation;

namespace AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;

public sealed class ConsulterPropositionsRecuesValidator : AbstractValidator<ConsulterPropositionsRecuesQuery>
{
    public ConsulterPropositionsRecuesValidator()
    {
        RuleFor(x => x.UtilisateurId).NotEmpty();
        RuleFor(x => x.AppelOffreId).NotEmpty();
    }
}
