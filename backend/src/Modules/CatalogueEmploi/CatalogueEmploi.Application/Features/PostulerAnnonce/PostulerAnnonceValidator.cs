using FluentValidation;

namespace CatalogueEmploi.Application.Features.PostulerAnnonce;

public sealed class PostulerAnnonceValidator : AbstractValidator<PostulerAnnonceCommand>
{
    public PostulerAnnonceValidator()
    {
        RuleFor(x => x.CandidatId).NotEmpty();
        RuleFor(x => x.AnnonceId).NotEmpty();
    }
}
