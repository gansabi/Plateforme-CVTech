using FluentValidation;

namespace CatalogueEmploi.Application.Features.CreerCurriculumVitae;

public sealed class CreerCurriculumVitaeValidator : AbstractValidator<CreerCurriculumVitaeCommand>
{
    public CreerCurriculumVitaeValidator()
    {
        RuleFor(x => x.CandidatId).NotEmpty();
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
    }
}
