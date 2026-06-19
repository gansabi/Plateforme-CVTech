using FluentValidation;

namespace CatalogueEmploi.Application.Features.ModifierCurriculumVitae;

public sealed class ModifierCurriculumVitaeValidator : AbstractValidator<ModifierCurriculumVitaeCommand>
{
    public ModifierCurriculumVitaeValidator()
    {
        RuleFor(x => x.CandidatId).NotEmpty();
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
    }
}
