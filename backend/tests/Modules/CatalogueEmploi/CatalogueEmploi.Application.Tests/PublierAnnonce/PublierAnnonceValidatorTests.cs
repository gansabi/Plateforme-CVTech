using CatalogueEmploi.Application.Features.PublierAnnonce;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace CatalogueEmploi.Application.Tests.PublierAnnonce;

public sealed class PublierAnnonceValidatorTests
{
    private readonly PublierAnnonceValidator _validator = new();

    [Fact]
    public void UneCommandeValidePasseLaValidation()
    {
        var command = new PublierAnnonceCommand
        {
            UtilisateurId = Guid.NewGuid(),
            Titre = "Développeur .NET Senior",
            Description = "Mission .NET 10 en architecture modulaire.",
            TypeContrat = "CDI",
            DomaineMetier = "Cloud Azure"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UneCommandeAvecTitreVideEchoueALaValidation(string titreInvalide)
    {
        var command = new PublierAnnonceCommand
        {
            UtilisateurId = Guid.NewGuid(),
            Titre = titreInvalide,
            Description = "Description valide.",
            TypeContrat = "CDI",
            DomaineMetier = "Cloud Azure"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PublierAnnonceCommand.Titre));
    }

    [Fact]
    public void UneCommandeAvecTypeContratInvalideEchoueALaValidation()
    {
        var command = new PublierAnnonceCommand
        {
            UtilisateurId = Guid.NewGuid(),
            Titre = "Titre valide",
            Description = "Description valide.",
            TypeContrat = "INCONNU",
            DomaineMetier = "Cloud Azure"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UneCommandeAvecUtilisateurIdVideEchoueALaValidation()
    {
        var command = new PublierAnnonceCommand
        {
            UtilisateurId = Guid.Empty,
            Titre = "Titre valide",
            Description = "Description valide.",
            TypeContrat = "CDI",
            DomaineMetier = "Cloud Azure"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
