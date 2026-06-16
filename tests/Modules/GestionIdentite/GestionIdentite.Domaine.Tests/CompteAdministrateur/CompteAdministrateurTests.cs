using FluentAssertions;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Xunit;

namespace GestionIdentite.Domaine.Tests.CompteAdministrateur;

public sealed class CompteAdministrateurTests
{
    private const string NomCompletValide = "Alice Martin";
    private const string MotDePasseHacheValide = "hash-pbkdf2";

    private static Email EmailValide() => Email.Creer("admin@exemple.fr");

    // -----------------------------------------------------------------------
    // Création — chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public void UnCompteAdministrateurEstCreéAvecLesInformationsValides()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), NomCompletValide, MotDePasseHacheValide);

        compte.Should().NotBeNull();
        compte.Id.Should().NotBe(Guid.Empty);
        compte.Email.Valeur.Should().Be("admin@exemple.fr");
        compte.NomComplet.Should().Be(NomCompletValide);
        compte.MotDePasseHache.Should().Be(MotDePasseHacheValide);
    }

    [Fact]
    public void UnCompteAdministrateurALeRoleAdministrateur()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), NomCompletValide, MotDePasseHacheValide);

        compte.Role.Should().Be(Role.Administrateur);
    }

    [Fact]
    public void UnNouveauCompteAdministrateurNEstPasBloque()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), NomCompletValide, MotDePasseHacheValide);

        compte.EstBloque.Should().BeFalse();
    }

    // -----------------------------------------------------------------------
    // Invariants métier
    // -----------------------------------------------------------------------

    [Fact]
    public void CreerAvecEmailNullLeveArgumentNullException()
    {
        var creer = () => Entites.CompteAdministrateur.Creer(null!, NomCompletValide, MotDePasseHacheValide);

        creer.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreerAvecNomCompletVideLeveNomCompletInvalideException(string nomInvalide)
    {
        var creer = () => Entites.CompteAdministrateur.Creer(EmailValide(), nomInvalide, MotDePasseHacheValide);

        creer.Should().Throw<NomCompletInvalideException>();
    }

    [Fact]
    public void LeNomCompletEstNormaliseEnSupprimantLesEspacesExterieurs()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), "  Alice Martin  ", MotDePasseHacheValide);

        compte.NomComplet.Should().Be("Alice Martin");
    }

    // -----------------------------------------------------------------------
    // Blocage / réactivation
    // -----------------------------------------------------------------------

    [Fact]
    public void BloquerUnCompteBloqueLeCompte()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), NomCompletValide, MotDePasseHacheValide);

        compte.Bloquer();

        compte.EstBloque.Should().BeTrue();
    }

    [Fact]
    public void BloquerUnCompteDejaBloqueLeveCompteDejaBloqueException()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), NomCompletValide, MotDePasseHacheValide);
        compte.Bloquer();

        var bloquer = () => compte.Bloquer();

        bloquer.Should().Throw<CompteDejaBloqueException>();
    }

    [Fact]
    public void ReactiverUnCompteBloqueLeReactive()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), NomCompletValide, MotDePasseHacheValide);
        compte.Bloquer();

        compte.Reactiver();

        compte.EstBloque.Should().BeFalse();
    }

    [Fact]
    public void ReactiverUnCompteNonBloqueLeveCompteNonBloqueException()
    {
        var compte = Entites.CompteAdministrateur.Creer(EmailValide(), NomCompletValide, MotDePasseHacheValide);

        var reactiver = () => compte.Reactiver();

        reactiver.Should().Throw<CompteNonBloqueException>();
    }
}
