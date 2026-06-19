using FluentAssertions;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Xunit;

namespace GestionIdentite.Domaine.Tests.CreerCompteCandidat;

public sealed class MotDePasseTests
{
    private const int LongueurMinimale = 8;

    // -----------------------------------------------------------------------
    // Création réussie
    // -----------------------------------------------------------------------

    [Fact]
    public void UnMotDePasseRespectantLesReglesEstAccepte()
    {
        // Arrange / Act
        var creer = () => MotDePasse.Creer("S3cr3t!Ok");

        // Assert
        creer.Should().NotThrow();
    }

    [Fact]
    public void UnMotDePasseDeLongueurMinimaleEstAccepte()
    {
        // Arrange — exactement 8 caractères, la limite basse autorisée
        var motDePasse8Caracteres = new string('A', LongueurMinimale - 1) + "1";

        // Act
        var creer = () => MotDePasse.Creer(motDePasse8Caracteres);

        // Assert
        creer.Should().NotThrow();
    }

    // -----------------------------------------------------------------------
    // Invariants — champ vide ou null
    // -----------------------------------------------------------------------

    [Fact]
    public void UnMotDePasseVideLeveUneExceptionMetier()
    {
        // Arrange / Act
        var creer = () => MotDePasse.Creer(string.Empty);

        // Assert
        creer.Should().Throw<MotDePasseInvalideException>()
            .WithMessage("*vide*");
    }

    [Fact]
    public void UnMotDePasseNullLeveUneExceptionMetier()
    {
        // Arrange / Act
        var creer = () => MotDePasse.Creer(null!);

        // Assert
        creer.Should().Throw<MotDePasseInvalideException>();
    }

    // -----------------------------------------------------------------------
    // Invariants — longueur minimale
    // -----------------------------------------------------------------------

    [Fact]
    public void UnMotDePasseTropCourtLeveUneExceptionMetier()
    {
        // Arrange — 7 caractères, en dessous du minimum de 8
        var motDePasseTropCourt = new string('A', LongueurMinimale - 1);

        // Act
        var creer = () => MotDePasse.Creer(motDePasseTropCourt);

        // Assert
        creer.Should().Throw<MotDePasseInvalideException>()
            .WithMessage($"*{LongueurMinimale}*");
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("abc123")]
    public void UnMotDePasseInferieurALaLongueurMinimaleLeveUneExceptionMetier(string motDePasseCourt)
    {
        // Arrange / Act
        var creer = () => MotDePasse.Creer(motDePasseCourt);

        // Assert
        creer.Should().Throw<MotDePasseInvalideException>();
    }

    // -----------------------------------------------------------------------
    // Sécurité — le mot de passe brut n'est pas exposé directement
    // -----------------------------------------------------------------------

    [Fact]
    public void LaRepresentationTextuelleNExposePasLeMotDePasseEnClair()
    {
        // Arrange
        const string valeurSecrete = "S3cr3t!Ok";
        var motDePasse = MotDePasse.Creer(valeurSecrete);

        // Act
        var representation = motDePasse.ToString();

        // Assert
        representation.Should().NotContain(valeurSecrete,
            because: "le mot de passe en clair ne doit jamais apparaître dans les logs ou traces");
    }
}
