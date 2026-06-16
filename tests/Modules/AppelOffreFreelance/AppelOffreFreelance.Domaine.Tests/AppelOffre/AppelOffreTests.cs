using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Enums;
using AppelOffreFreelance.Domaine.Exceptions;
using AppelOffreFreelance.Domaine.ObjetsValeur;
using FluentAssertions;
using Xunit;

namespace AppelOffreFreelance.Domaine.Tests.AppelOffre;

public sealed class AppelOffreTests
{
    private static readonly Guid EntrepriseId = Guid.NewGuid();
    private const string TitreValide = "Mission Architecture Cloud Azure";
    private const string DescriptionValide = "Conception et déploiement d'une infrastructure cloud.";
    private static DomaineMetier DomaineValide() => DomaineMetier.Creer("Cloud Azure");
    private static ObjetsValeur.BaremeTJM BaremeValide() => ObjetsValeur.BaremeTJM.Creer(500, 800);

    [Fact]
    public void UnAppelOffreEstCreeAvecLesInformationsValides()
    {
        var appelOffre = Entites.AppelOffre.Publier(
            EntrepriseId, TitreValide, DescriptionValide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        appelOffre.Should().NotBeNull();
        appelOffre.Id.Should().NotBe(Guid.Empty);
        appelOffre.EntrepriseId.Should().Be(EntrepriseId);
        appelOffre.Titre.Should().Be(TitreValide);
        appelOffre.Description.Should().Be(DescriptionValide);
    }

    [Fact]
    public void UnNouvelAppelOffreEstOuvertALaCreation()
    {
        var appelOffre = Entites.AppelOffre.Publier(
            EntrepriseId, TitreValide, DescriptionValide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        appelOffre.Statut.Should().Be(StatutAppelOffre.Ouvert);
    }

    [Fact]
    public void UnAppelOffreAUneDateDePublicationRenseignee()
    {
        var avant = DateTime.UtcNow;
        var appelOffre = Entites.AppelOffre.Publier(
            EntrepriseId, TitreValide, DescriptionValide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        appelOffre.DatePublication.Should().BeOnOrAfter(avant);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void PublierAvecTitreVideLeveTitreAppelOffreInvalideException(string titreInvalide)
    {
        var publier = () => Entites.AppelOffre.Publier(
            EntrepriseId, titreInvalide, DescriptionValide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        publier.Should().Throw<TitreAppelOffreInvalideException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void PublierAvecDescriptionVideLeveDescriptionAppelOffreInvalideException(string descInvalide)
    {
        var publier = () => Entites.AppelOffre.Publier(
            EntrepriseId, TitreValide, descInvalide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        publier.Should().Throw<DescriptionAppelOffreInvalideException>();
    }

    [Fact]
    public void PublierAvecEntrepriseIdVideLeveArgumentException()
    {
        var publier = () => Entites.AppelOffre.Publier(
            Guid.Empty, TitreValide, DescriptionValide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        publier.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ModererUnAppelOffreFermeLAppelOffre()
    {
        var appelOffre = Entites.AppelOffre.Publier(
            EntrepriseId, TitreValide, DescriptionValide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        appelOffre.Moderer();

        appelOffre.Statut.Should().Be(StatutAppelOffre.Ferme);
    }

    [Fact]
    public void SupprimerUnAppelOffreFermeLAppelOffre()
    {
        var appelOffre = Entites.AppelOffre.Publier(
            EntrepriseId, TitreValide, DescriptionValide,
            DomaineValide(), BaremeValide(), DateTime.UtcNow.AddDays(30));

        appelOffre.Supprimer();

        appelOffre.Statut.Should().Be(StatutAppelOffre.Ferme);
    }
}
