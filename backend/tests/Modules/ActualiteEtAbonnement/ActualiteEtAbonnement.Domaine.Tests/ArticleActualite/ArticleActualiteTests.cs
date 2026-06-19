using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Exceptions;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using FluentAssertions;
using Xunit;

namespace ActualiteEtAbonnement.Domaine.Tests.ArticleActualite;

public sealed class ArticleActualiteTests
{
    private static readonly Guid AuteurId = Guid.NewGuid();
    private const string TitreValide = "Les tendances du Cloud en 2026";
    private const string ContenuValide = "Le cloud continue sa progression avec de nouvelles offres serverless.";
    private static DomaineMetier DomaineValide() => DomaineMetier.Creer("Cloud Azure");

    [Fact]
    public void UnArticleEstCreeAvecLesInformationsValides()
    {
        var article = Entites.ArticleActualite.Publier(
            AuteurId, TitreValide, ContenuValide, DomaineValide());

        article.Should().NotBeNull();
        article.Id.Should().NotBe(Guid.Empty);
        article.AuteurId.Should().Be(AuteurId);
        article.Titre.Should().Be(TitreValide);
        article.Contenu.Should().Be(ContenuValide);
        article.DomaineMetier.Valeur.Should().Be("Cloud Azure");
    }

    [Fact]
    public void UnNouvelArticleAUneDateDePublicationRenseignee()
    {
        var avant = DateTime.UtcNow;
        var article = Entites.ArticleActualite.Publier(
            AuteurId, TitreValide, ContenuValide, DomaineValide());

        article.DatePublication.Should().BeOnOrAfter(avant);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void PublierAvecTitreVideLeveTitreArticleInvalideException(string titreInvalide)
    {
        var publier = () => Entites.ArticleActualite.Publier(
            AuteurId, titreInvalide, ContenuValide, DomaineValide());

        publier.Should().Throw<TitreArticleInvalideException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void PublierAvecContenuVideLeveContenuArticleInvalideException(string contenuInvalide)
    {
        var publier = () => Entites.ArticleActualite.Publier(
            AuteurId, TitreValide, contenuInvalide, DomaineValide());

        publier.Should().Throw<ContenuArticleInvalideException>();
    }

    [Fact]
    public void PublierAvecAuteurIdVideLeveArgumentException()
    {
        var publier = () => Entites.ArticleActualite.Publier(
            Guid.Empty, TitreValide, ContenuValide, DomaineValide());

        publier.Should().Throw<ArgumentException>();
    }
}
