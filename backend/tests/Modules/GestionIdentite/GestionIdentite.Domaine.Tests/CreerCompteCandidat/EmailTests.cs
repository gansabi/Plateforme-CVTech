using FluentAssertions;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Xunit;

namespace GestionIdentite.Domaine.Tests.CreerCompteCandidat;

public sealed class EmailTests
{
    // -----------------------------------------------------------------------
    // Création réussie
    // -----------------------------------------------------------------------

    [Fact]
    public void UnEmailValideEstAccepte()
    {
        // Arrange / Act
        var creer = () => Email.Creer("candidat@exemple.fr");

        // Assert
        creer.Should().NotThrow();
    }

    [Theory]
    [InlineData("a@b.fr")]
    [InlineData("prenom.nom@entreprise.com")]
    [InlineData("user+tag@domaine.org")]
    public void DifferentesAdressesEmailValidesSOntAcceptees(string adresse)
    {
        // Arrange / Act
        var creer = () => Email.Creer(adresse);

        // Assert
        creer.Should().NotThrow();
    }

    [Fact]
    public void LAdresseEmailEstConserveeApresCreation()
    {
        // Arrange
        const string adresse = "candidat@exemple.fr";

        // Act
        var email = Email.Creer(adresse);

        // Assert
        email.Valeur.Should().Be(adresse);
    }

    // -----------------------------------------------------------------------
    // Égalité par valeur (objet valeur)
    // -----------------------------------------------------------------------

    [Fact]
    public void DeuxEmailsAvecLaMemeAdresseSontEgaux()
    {
        // Arrange
        var email1 = Email.Creer("candidat@exemple.fr");
        var email2 = Email.Creer("candidat@exemple.fr");

        // Assert
        email1.Should().Be(email2);
    }

    [Fact]
    public void DeuxEmailsAvecDesAdressesDifferentesSontDifferents()
    {
        // Arrange
        var email1 = Email.Creer("alice@exemple.fr");
        var email2 = Email.Creer("bob@exemple.fr");

        // Assert
        email1.Should().NotBe(email2);
    }

    [Fact]
    public void LComparaisonDesEmailsEstInsensibleALaCasse()
    {
        // Arrange
        var email1 = Email.Creer("Candidat@Exemple.FR");
        var email2 = Email.Creer("candidat@exemple.fr");

        // Assert
        email1.Should().Be(email2);
    }

    // -----------------------------------------------------------------------
    // Invariants — format invalide
    // -----------------------------------------------------------------------

    [Fact]
    public void UnEmailVideLeveUneExceptionMetier()
    {
        // Arrange / Act
        var creer = () => Email.Creer(string.Empty);

        // Assert
        creer.Should().Throw<EmailInvalideException>()
            .WithMessage("*vide*");
    }

    [Fact]
    public void UnEmailNullLeveUneExceptionMetier()
    {
        // Arrange / Act
        var creer = () => Email.Creer(null!);

        // Assert
        creer.Should().Throw<EmailInvalideException>();
    }

    [Fact]
    public void UnEmailSansArrobaseLeveUneExceptionMetier()
    {
        // Arrange / Act
        var creer = () => Email.Creer("candidatexemple.fr");

        // Assert
        creer.Should().Throw<EmailInvalideException>()
            .WithMessage("*format*");
    }

    [Fact]
    public void UnEmailSansDomaineLeveUneExceptionMetier()
    {
        // Arrange / Act
        var creer = () => Email.Creer("candidat@");

        // Assert
        creer.Should().Throw<EmailInvalideException>();
    }

    [Fact]
    public void UnEmailSansExtensionLeveUneExceptionMetier()
    {
        // Arrange / Act
        var creer = () => Email.Creer("candidat@exemple");

        // Assert
        creer.Should().Throw<EmailInvalideException>();
    }

    [Theory]
    [InlineData("@exemple.fr")]
    [InlineData("candidat@.fr")]
    [InlineData("  ")]
    [InlineData("candidat @exemple.fr")]
    public void UnEmailMalFormeLeveUneExceptionMetier(string adresseInvalide)
    {
        // Arrange / Act
        var creer = () => Email.Creer(adresseInvalide);

        // Assert
        creer.Should().Throw<EmailInvalideException>();
    }
}
