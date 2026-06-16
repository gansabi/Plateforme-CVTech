using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Enums;
using CatalogueEmploi.Domaine.Exceptions;
using CatalogueEmploi.Domaine.ObjetsValeur;
using FluentAssertions;
using Xunit;

namespace CatalogueEmploi.Domaine.Tests.AnnonceEmploi;

public sealed class AnnonceEmploiTests
{
    private static readonly Guid EntrepriseId = Guid.NewGuid();
    private const string TitreValide = "Développeur .NET Senior";
    private const string DescriptionValide = "Mission de développement .NET 10 en architecture modulaire.";
    private static DomaineMetier DomaineValide() => DomaineMetier.Creer("Cloud Azure");

    // -----------------------------------------------------------------------
    // Création — chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public void UneAnnonceEstCreeeAvecLesInformationsValides()
    {
        var annonce = Entites.AnnonceEmploi.Publier(
            EntrepriseId, TitreValide, DescriptionValide, TypeContrat.CDI, DomaineValide());

        annonce.Should().NotBeNull();
        annonce.Id.Should().NotBe(Guid.Empty);
        annonce.EntrepriseId.Should().Be(EntrepriseId);
        annonce.Titre.Should().Be(TitreValide);
        annonce.Description.Should().Be(DescriptionValide);
        annonce.TypeContrat.Should().Be(TypeContrat.CDI);
    }

    [Fact]
    public void UneNouvelleAnnonceEstActiveALaCreation()
    {
        var annonce = Entites.AnnonceEmploi.Publier(
            EntrepriseId, TitreValide, DescriptionValide, TypeContrat.CDI, DomaineValide());

        annonce.EstActive.Should().BeTrue();
    }

    [Fact]
    public void UneAnnonceAUneDateDePublicationRenseignee()
    {
        var avant = DateTime.UtcNow;
        var annonce = Entites.AnnonceEmploi.Publier(
            EntrepriseId, TitreValide, DescriptionValide, TypeContrat.CDI, DomaineValide());

        annonce.DatePublication.Should().BeOnOrAfter(avant);
    }

    // -----------------------------------------------------------------------
    // Invariants métier
    // -----------------------------------------------------------------------

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void PublierAvecTitreVideLeveTitreAnnonceInvalideException(string titreInvalide)
    {
        var publier = () => Entites.AnnonceEmploi.Publier(
            EntrepriseId, titreInvalide, DescriptionValide, TypeContrat.CDI, DomaineValide());

        publier.Should().Throw<TitreAnnonceInvalideException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void PublierAvecDescriptionVideLeveDescriptionAnnonceInvalideException(string descriptionInvalide)
    {
        var publier = () => Entites.AnnonceEmploi.Publier(
            EntrepriseId, TitreValide, descriptionInvalide, TypeContrat.CDI, DomaineValide());

        publier.Should().Throw<DescriptionAnnonceInvalideException>();
    }

    [Fact]
    public void PublierAvecEntrepriseIdVideLeveArgumentException()
    {
        var publier = () => Entites.AnnonceEmploi.Publier(
            Guid.Empty, TitreValide, DescriptionValide, TypeContrat.CDI, DomaineValide());

        publier.Should().Throw<ArgumentException>();
    }

    // -----------------------------------------------------------------------
    // Modération
    // -----------------------------------------------------------------------

    [Fact]
    public void ModererUneAnnonceDesactiveLeAnnonce()
    {
        var annonce = Entites.AnnonceEmploi.Publier(
            EntrepriseId, TitreValide, DescriptionValide, TypeContrat.CDI, DomaineValide());

        annonce.Moderer();

        annonce.EstActive.Should().BeFalse();
    }

    [Fact]
    public void SupprimerUneAnnonceDesactiveLAnnonce()
    {
        var annonce = Entites.AnnonceEmploi.Publier(
            EntrepriseId, TitreValide, DescriptionValide, TypeContrat.CDI, DomaineValide());

        annonce.Supprimer();

        annonce.EstActive.Should().BeFalse();
    }
}
