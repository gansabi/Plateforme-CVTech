using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Exceptions;
using FluentAssertions;
using Xunit;

namespace CatalogueEmploi.Domaine.Tests.CurriculumVitae;

public sealed class CurriculumVitaeTests
{
    private static readonly Guid CandidatId = Guid.NewGuid();
    private const string TitreValide = "Développeur .NET Senior";
    private const string ResumeValide = "Cinq ans d'expérience sur des projets enterprise .NET.";
    private static readonly IEnumerable<string> CompetencesValides = ["C#", ".NET", "Azure"];

    // -----------------------------------------------------------------------
    // Création — chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public void UnCurriculumVitaeEstCreeeAvecLesInformationsValides()
    {
        var cv = Entites.CurriculumVitae.Creer(CandidatId, TitreValide, ResumeValide, CompetencesValides);

        cv.Should().NotBeNull();
        cv.Id.Should().NotBe(Guid.Empty);
        cv.CandidatId.Should().Be(CandidatId);
        cv.Titre.Should().Be(TitreValide);
        cv.Resume.Should().Be(ResumeValide);
        cv.CompetencesPrincipales.Should().BeEquivalentTo(CompetencesValides);
    }

    [Fact]
    public void UnNouveauCVAUneDateDeCreationRenseignee()
    {
        var avant = DateTime.UtcNow;
        var cv = Entites.CurriculumVitae.Creer(CandidatId, TitreValide, ResumeValide, CompetencesValides);

        cv.DateCreation.Should().BeOnOrAfter(avant);
    }

    // -----------------------------------------------------------------------
    // Invariants
    // -----------------------------------------------------------------------

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreerAvecTitreVideLeveTitreCVInvalideException(string titreInvalide)
    {
        var creer = () => Entites.CurriculumVitae.Creer(CandidatId, titreInvalide, ResumeValide, CompetencesValides);

        creer.Should().Throw<TitreCVInvalideException>();
    }

    [Fact]
    public void CreerAvecCandidatIdVideLeveArgumentException()
    {
        var creer = () => Entites.CurriculumVitae.Creer(Guid.Empty, TitreValide, ResumeValide, CompetencesValides);

        creer.Should().Throw<ArgumentException>();
    }

    // -----------------------------------------------------------------------
    // Modification
    // -----------------------------------------------------------------------

    [Fact]
    public void ModifierUnCVMiseAJourLesTitreEtResume()
    {
        var cv = Entites.CurriculumVitae.Creer(CandidatId, TitreValide, ResumeValide, CompetencesValides);
        var nouvelleDateAvant = DateTime.UtcNow;

        cv.Modifier("Architecte Cloud", "Expert Azure et .NET.", ["Azure", "Terraform"]);

        cv.Titre.Should().Be("Architecte Cloud");
        cv.Resume.Should().Be("Expert Azure et .NET.");
        cv.CompetencesPrincipales.Should().BeEquivalentTo(["Azure", "Terraform"]);
        cv.DateMiseAJour.Should().BeOnOrAfter(nouvelleDateAvant);
    }
}
