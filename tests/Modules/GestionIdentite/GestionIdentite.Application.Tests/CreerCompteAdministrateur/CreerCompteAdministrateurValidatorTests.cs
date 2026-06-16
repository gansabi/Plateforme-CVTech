using FluentAssertions;
using FluentValidation.TestHelper;
using GestionIdentite.Application.Features.CreerCompteAdministrateur;
using Xunit;

namespace GestionIdentite.Application.Tests.CreerCompteAdministrateur;

public sealed class CreerCompteAdministrateurValidatorTests
{
    private readonly CreerCompteAdministrateurValidator _validator = new();

    [Fact]
    public void UneCommandeValidePasseLaValidation()
    {
        var command = new CreerCompteAdministrateurCommand
        {
            Email = "admin@exemple.fr",
            NomComplet = "Alice Martin",
            MotDePasse = "S3cur3Admin!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("pas_un_email")]
    [InlineData("espace @exemple.fr")]
    public void UneCommandeAvecEmailInvalideEchoueALaValidation(string emailInvalide)
    {
        var command = new CreerCompteAdministrateurCommand
        {
            Email = emailInvalide,
            NomComplet = "Alice Martin",
            MotDePasse = "S3cur3Admin!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UneCommandeAvecNomCompletVideEchoueALaValidation(string nomInvalide)
    {
        var command = new CreerCompteAdministrateurCommand
        {
            Email = "admin@exemple.fr",
            NomComplet = nomInvalide,
            MotDePasse = "S3cur3Admin!"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.NomComplet);
    }

    [Fact]
    public void UneCommandeAvecMotDePasseTropCourtEchoueALaValidation()
    {
        var command = new CreerCompteAdministrateurCommand
        {
            Email = "admin@exemple.fr",
            NomComplet = "Alice Martin",
            MotDePasse = "court"
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.MotDePasse);
    }
}
