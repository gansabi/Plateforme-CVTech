using FluentAssertions;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Xunit;

namespace GestionIdentite.Domaine.Tests.CompteEntreprise;

public sealed class CompteEntrepriseTests
{
    private static readonly Email EmailValide = Email.Creer("entreprise@exemple.fr");
    private const string NomEntrepriseValide = "TechCorp SAS";
    private const string HashValide = "hash_bcrypt_exemple";

    // -----------------------------------------------------------------------
    // Création
    // -----------------------------------------------------------------------

    [Fact]
    public void LaCreationDUnCompteEntrepriseAvecDesParametresValidesReussit()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);

        compte.Should().NotBeNull();
    }

    [Fact]
    public void LaCreationAttribueUnIdentifiantUnique()
    {
        var compte1 = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);
        var compte2 = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);

        compte1.Id.Should().NotBe(compte2.Id);
    }

    [Fact]
    public void LaCreationAttribueLeRoleEntreprise()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);

        compte.Role.Should().Be(Role.Entreprise);
    }

    [Fact]
    public void UnCompteEntrepriseNouvelNEstPasBloque()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);

        compte.EstBloque.Should().BeFalse();
    }

    [Fact]
    public void LaCreationAvecEmailNullLeveUneException()
    {
        var creer = () => Entites.CompteEntreprise.Creer(null!, NomEntrepriseValide, HashValide);

        creer.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void LaCreationAvecNomEntrepriseVideLeveUneNomEntrepriseInvalideException()
    {
        var creer = () => Entites.CompteEntreprise.Creer(EmailValide, string.Empty, HashValide);

        creer.Should().Throw<NomEntrepriseInvalideException>();
    }

    [Fact]
    public void LaCreationAvecNomEntrepriseNullLeveUneNomEntrepriseInvalideException()
    {
        var creer = () => Entites.CompteEntreprise.Creer(EmailValide, null!, HashValide);

        creer.Should().Throw<NomEntrepriseInvalideException>();
    }

    [Fact]
    public void LaCreationStockeLeMotDePasseHache()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);

        compte.MotDePasseHache.Should().Be(HashValide);
    }

    // -----------------------------------------------------------------------
    // Blocage
    // -----------------------------------------------------------------------

    [Fact]
    public void UnAdministrateurPeutBloquerUnCompteEntreprise()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);

        compte.Bloquer();

        compte.EstBloque.Should().BeTrue();
    }

    [Fact]
    public void BloquerUnCompteDejaBloqueLeveuneCompteDejaBloqueException()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);
        compte.Bloquer();

        var bloquer = () => compte.Bloquer();

        bloquer.Should().Throw<CompteDejaBloqueException>();
    }

    // -----------------------------------------------------------------------
    // Réactivation
    // -----------------------------------------------------------------------

    [Fact]
    public void UnAdministrateurPeutReactiverUnCompteEntrepriseBloque()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);
        compte.Bloquer();

        compte.Reactiver();

        compte.EstBloque.Should().BeFalse();
    }

    [Fact]
    public void ReactiverUnCompteNonBloqueLeveuneCompteNonBloqueException()
    {
        var compte = Entites.CompteEntreprise.Creer(EmailValide, NomEntrepriseValide, HashValide);

        var reactiver = () => compte.Reactiver();

        reactiver.Should().Throw<CompteNonBloqueException>();
    }
}
