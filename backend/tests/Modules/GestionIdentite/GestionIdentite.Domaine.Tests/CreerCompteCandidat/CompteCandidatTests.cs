using FluentAssertions;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Xunit;

namespace GestionIdentite.Domaine.Tests.CreerCompteCandidat;

/// <summary>
/// Tests de spécification du Domaine — Phase RED.
/// Aucun code de production n'existe encore : ces tests DOIVENT échouer à la compilation.
/// Cf. .agent/skills/regles-tdd.md — cycle Red → Green → Refactor
/// </summary>
public sealed class CompteCandidatTests
{
    // -----------------------------------------------------------------------
    // Helpers de construction — centralisés pour ne pas dupliquer les valeurs
    // -----------------------------------------------------------------------

    private static Email UnEmailValide() => Email.Creer("candidat@exemple.fr");
    private static MotDePasse UnMotDePasseValide() => MotDePasse.Creer("S3cr3t!Ok");

    // -----------------------------------------------------------------------
    // Création réussie
    // -----------------------------------------------------------------------

    [Fact]
    public void UnCandidatPeutCreerUnCompteAvecDesInformationsValides()
    {
        // Arrange
        var email = UnEmailValide();
        var motDePasse = UnMotDePasseValide();

        // Act
        var compte = CompteCandidat.Creer(email, motDePasse);

        // Assert
        compte.Should().NotBeNull();
    }

    [Fact]
    public void UnNouveauCompteCandidatALeRoleCandidat()
    {
        // Arrange / Act
        var compte = CompteCandidat.Creer(UnEmailValide(), UnMotDePasseValide());

        // Assert
        compte.Role.Should().Be(Role.Candidat);
    }

    [Fact]
    public void UnNouveauCompteCandidatEstActifParDefaut()
    {
        // Arrange / Act
        var compte = CompteCandidat.Creer(UnEmailValide(), UnMotDePasseValide());

        // Assert
        compte.EstBloque.Should().BeFalse();
    }

    [Fact]
    public void UnNouveauCompteCandidatPossedeUnIdentifiantNonVide()
    {
        // Arrange / Act
        var compte = CompteCandidat.Creer(UnEmailValide(), UnMotDePasseValide());

        // Assert
        compte.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void DeuxComptesCreesIndependammentOntDesIdentifiantsDifferents()
    {
        // Arrange / Act
        var premierCompte = CompteCandidat.Creer(
            Email.Creer("premier@exemple.fr"),
            UnMotDePasseValide());

        var secondCompte = CompteCandidat.Creer(
            Email.Creer("second@exemple.fr"),
            UnMotDePasseValide());

        // Assert
        premierCompte.Id.Should().NotBe(secondCompte.Id);
    }

    [Fact]
    public void UnNouveauCompteCandidatConserveLeMailFourniALaCreation()
    {
        // Arrange
        var email = Email.Creer("candidat@exemple.fr");

        // Act
        var compte = CompteCandidat.Creer(email, UnMotDePasseValide());

        // Assert
        compte.Email.Should().Be(email);
    }

    // -----------------------------------------------------------------------
    // Invariants — email obligatoire
    // -----------------------------------------------------------------------

    [Fact]
    public void UnCompteNePeutPasEtreCreeSansEmail()
    {
        // Arrange / Act
        var creer = () => CompteCandidat.Creer(null!, UnMotDePasseValide());

        // Assert
        creer.Should().Throw<ArgumentNullException>();
    }

    // -----------------------------------------------------------------------
    // Invariants — mot de passe obligatoire
    // -----------------------------------------------------------------------

    [Fact]
    public void UnCompteNePeutPasEtreCreeSansMotDePasse()
    {
        // Arrange / Act
        var creer = () => CompteCandidat.Creer(UnEmailValide(), null!);

        // Assert
        creer.Should().Throw<ArgumentNullException>();
    }

    // -----------------------------------------------------------------------
    // Règles de gestion — blocage de compte
    // -----------------------------------------------------------------------

    [Fact]
    public void UnCompteCandidatPeutEtreBloqueParUnAdministrateur()
    {
        // Arrange
        var compte = CompteCandidat.Creer(UnEmailValide(), UnMotDePasseValide());

        // Act
        compte.Bloquer();

        // Assert
        compte.EstBloque.Should().BeTrue();
    }

    [Fact]
    public void UnCompteDejaBloqueNePeutPasEtreBloqueSuiteAUneNouvelleAction()
    {
        // Arrange
        var compte = CompteCandidat.Creer(UnEmailValide(), UnMotDePasseValide());
        compte.Bloquer();

        // Act
        var bloquerDeNouveau = () => compte.Bloquer();

        // Assert
        bloquerDeNouveau.Should().Throw<CompteDejaBloqueException>();
    }

    [Fact]
    public void UnCompteBloqueRedevientActifApresReactivation()
    {
        // Arrange
        var compte = CompteCandidat.Creer(UnEmailValide(), UnMotDePasseValide());
        compte.Bloquer();

        // Act
        compte.Reactiver();

        // Assert
        compte.EstBloque.Should().BeFalse();
    }

    [Fact]
    public void UnCompteNonBloqueNePeutPasEtreReactive()
    {
        // Arrange
        var compte = CompteCandidat.Creer(UnEmailValide(), UnMotDePasseValide());

        // Act
        var reactiver = () => compte.Reactiver();

        // Assert
        reactiver.Should().Throw<CompteNonBloqueException>();
    }
}
