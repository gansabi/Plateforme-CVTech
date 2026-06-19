using FluentValidation;

namespace CatalogueEmploi.Application.Features.SupprimerAnnonce;

public sealed class SupprimerAnnonceValidator : AbstractValidator<SupprimerAnnonceCommand>
{
    public SupprimerAnnonceValidator()
    {
        RuleFor(x => x.AdministrateurId).NotEmpty();
        RuleFor(x => x.AnnonceId).NotEmpty();
    }
}
