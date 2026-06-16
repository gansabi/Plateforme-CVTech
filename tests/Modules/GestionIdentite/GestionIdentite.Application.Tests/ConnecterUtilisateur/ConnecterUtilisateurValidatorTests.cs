using FluentValidation.TestHelper;
using GestionIdentite.Application.Features.ConnecterUtilisateur;
using Xunit;

namespace GestionIdentite.Application.Tests.ConnecterUtilisateur;

public sealed class ConnecterUtilisateurValidatorTests
{
    private readonly ConnecterUtilisateurValidator _validator = new();

    [Fact]
    public void UneCommandeValideNaAucuneErreur()
    {
        var command = new ConnecterUtilisateurCommand
        {
            Email = "user@exemple.fr",
            MotDePasse = "S3cr3t!Ok"
        };

        _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UnEmailVideProduiteUneErreurDeValidation()
    {
        var command = new ConnecterUtilisateurCommand
        {
            Email = string.Empty,
            MotDePasse = "S3cr3t!Ok"
        };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void UnMotDePasseVideProduiteUneErreurDeValidation()
    {
        var command = new ConnecterUtilisateurCommand
        {
            Email = "user@exemple.fr",
            MotDePasse = string.Empty
        };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.MotDePasse);
    }
}
