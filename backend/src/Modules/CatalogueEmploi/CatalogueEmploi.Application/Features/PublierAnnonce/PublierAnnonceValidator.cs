using CatalogueEmploi.Domaine.Enums;
using FluentValidation;

namespace CatalogueEmploi.Application.Features.PublierAnnonce;

public sealed class PublierAnnonceValidator : AbstractValidator<PublierAnnonceCommand>
{
    private static readonly string[] TypesContratValides =
        Enum.GetNames<TypeContrat>();

    public PublierAnnonceValidator()
    {
        RuleFor(x => x.UtilisateurId).NotEmpty();
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.DomaineMetier).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TypeContrat)
            .NotEmpty()
            .Must(t => TypesContratValides.Contains(t, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Le type de contrat doit être l'un des suivants : {string.Join(", ", TypesContratValides)}.");
    }
}
