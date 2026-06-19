using FluentValidation;

namespace CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;

public sealed class ConsulterCandidaturesRecuesValidator : AbstractValidator<ConsulterCandidaturesRecuesQuery>
{
    public ConsulterCandidaturesRecuesValidator()
    {
        RuleFor(x => x.UtilisateurId).NotEmpty();
        RuleFor(x => x.AnnonceId).NotEmpty();
    }
}
