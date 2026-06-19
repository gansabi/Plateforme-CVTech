using FluentValidation;

namespace CatalogueEmploi.Application.Features.ModererAnnonce;

public sealed class ModererAnnonceValidator : AbstractValidator<ModererAnnonceCommand>
{
    public ModererAnnonceValidator()
    {
        RuleFor(x => x.AdministrateurId).NotEmpty();
        RuleFor(x => x.AnnonceId).NotEmpty();
    }
}
