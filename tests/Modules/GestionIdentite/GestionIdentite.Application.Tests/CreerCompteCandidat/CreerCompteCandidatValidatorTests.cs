using FluentAssertions;
using FluentValidation.TestHelper;
using GestionIdentite.Application.Features.CreerCompteCandidat;
using Xunit;

namespace GestionIdentite.Application.Tests.CreerCompteCandidat;

public sealed class CreerCompteCandidatValidatorTests
{
    private readonly CreerCompteCandidatValidator _validator = new();

    // -----------------------------------------------------------------------
    // Chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public void UneCommandeAvecToutesLesValeursValidesNaAucuneErreur()
    {
        // Arrange
        var command = new CreerCompteCandidatCommand
        {
            Email = "candidat@exemple.fr",
            MotDePasse = "S3cr3t!Ok"
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldNotHaveAnyValidationErrors();
    }

    // -----------------------------------------------------------------------
    // Validation de l'Email
    // -----------------------------------------------------------------------

    [Fact]
    public void UneCommandeAvecEmailVideEchoueALaValidation()
    {
        // Arrange
        var command = new CreerCompteCandidatCommand
        {
            Email = string.Empty,
            MotDePasse = "S3cr3t!Ok"
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void UneCommandeAvecEmailNullEchoueALaValidation()
    {
        // Arrange
        var command = new CreerCompteCandidatCommand
        {
            Email = null!,
            MotDePasse = "S3cr3t!Ok"
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("pasUnEmail")]
    [InlineData("@sans-local.fr")]
    [InlineData("sans-arobase.fr")]
    [InlineData("espace @exemple.fr")]
    public void UneCommandeAvecEmailMalFormeEchoueALaValidation(string emailInvalide)
    {
        // Arrange
        var command = new CreerCompteCandidatCommand
        {
            Email = emailInvalide,
            MotDePasse = "S3cr3t!Ok"
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldHaveValidationErrorFor(x => x.Email);
    }

    // -----------------------------------------------------------------------
    // Validation du MotDePasse
    // -----------------------------------------------------------------------

    [Fact]
    public void UneCommandeAvecMotDePasseVideEchoueALaValidation()
    {
        // Arrange
        var command = new CreerCompteCandidatCommand
        {
            Email = "candidat@exemple.fr",
            MotDePasse = string.Empty
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldHaveValidationErrorFor(x => x.MotDePasse);
    }

    [Fact]
    public void UneCommandeAvecMotDePasseNullEchoueALaValidation()
    {
        // Arrange
        var command = new CreerCompteCandidatCommand
        {
            Email = "candidat@exemple.fr",
            MotDePasse = null!
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldHaveValidationErrorFor(x => x.MotDePasse);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("1234567")]   // 7 chars — en dessous du minimum de 8
    public void UneCommandeAvecMotDePasseTropCourtEchoueALaValidation(string motDePasseCourt)
    {
        // Arrange
        var command = new CreerCompteCandidatCommand
        {
            Email = "candidat@exemple.fr",
            MotDePasse = motDePasseCourt
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldHaveValidationErrorFor(x => x.MotDePasse);
    }

    [Fact]
    public void UneCommandeAvecMotDePasseDeLongueurMinimaleEstValide()
    {
        // Arrange — exactement 8 caractères
        var command = new CreerCompteCandidatCommand
        {
            Email = "candidat@exemple.fr",
            MotDePasse = "AAAAAAA1"
        };

        // Act
        var resultat = _validator.TestValidate(command);

        // Assert
        resultat.ShouldNotHaveValidationErrorFor(x => x.MotDePasse);
    }
}
