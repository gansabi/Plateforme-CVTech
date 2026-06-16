using FluentAssertions;
using FluentValidation.TestHelper;
using GestionIdentite.Application.Features.CreerCompteEntreprise;
using Xunit;

namespace GestionIdentite.Application.Tests.CreerCompteEntreprise;

public sealed class CreerCompteEntrepriseValidatorTests
{
    private readonly CreerCompteEntrepriseValidator _validator = new();

    [Fact]
    public void UneCommandeValideNaAucuneErreur()
    {
        var command = new CreerCompteEntrepriseCommand
        {
            Email = "entreprise@exemple.fr",
            MotDePasse = "S3cr3t!Ok",
            NomEntreprise = "TechCorp SAS"
        };

        var resultat = _validator.TestValidate(command);

        resultat.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UnEmailVideProduiteUneErreurDeValidation()
    {
        var command = new CreerCompteEntrepriseCommand
        {
            Email = string.Empty,
            MotDePasse = "S3cr3t!Ok",
            NomEntreprise = "TechCorp SAS"
        };

        var resultat = _validator.TestValidate(command);

        resultat.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void UnEmailMalFormeProduiteUneErreurDeValidation()
    {
        var command = new CreerCompteEntrepriseCommand
        {
            Email = "pas_un_email",
            MotDePasse = "S3cr3t!Ok",
            NomEntreprise = "TechCorp SAS"
        };

        var resultat = _validator.TestValidate(command);

        resultat.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void UnMotDePasseTropCourtProduiteUneErreurDeValidation()
    {
        var command = new CreerCompteEntrepriseCommand
        {
            Email = "entreprise@exemple.fr",
            MotDePasse = "court",
            NomEntreprise = "TechCorp SAS"
        };

        var resultat = _validator.TestValidate(command);

        resultat.ShouldHaveValidationErrorFor(c => c.MotDePasse);
    }

    [Fact]
    public void UnNomEntrepriseVideProduiteUneErreurDeValidation()
    {
        var command = new CreerCompteEntrepriseCommand
        {
            Email = "entreprise@exemple.fr",
            MotDePasse = "S3cr3t!Ok",
            NomEntreprise = string.Empty
        };

        var resultat = _validator.TestValidate(command);

        resultat.ShouldHaveValidationErrorFor(c => c.NomEntreprise);
    }
}
