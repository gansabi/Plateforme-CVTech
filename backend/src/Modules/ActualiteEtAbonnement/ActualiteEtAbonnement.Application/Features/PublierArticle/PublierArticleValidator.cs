using FluentValidation;

namespace ActualiteEtAbonnement.Application.Features.PublierArticle;

public sealed class PublierArticleValidator : AbstractValidator<PublierArticleCommand>
{
    public PublierArticleValidator()
    {
        RuleFor(x => x.AuteurId).NotEmpty();
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Contenu).NotEmpty().MaximumLength(10000);
        RuleFor(x => x.DomaineMetier).NotEmpty().MaximumLength(100);
    }
}
