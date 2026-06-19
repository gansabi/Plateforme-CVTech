using CatalogueEmploi.Domaine.Exceptions;
using FluentAssertions;
using Xunit;

namespace CatalogueEmploi.Domaine.Tests.Candidature;

public sealed class CandidatureTests
{
    private static readonly Guid CandidatId = Guid.NewGuid();
    private static readonly Guid AnnonceId = Guid.NewGuid();

    [Fact]
    public void UneCandidatureEstCreeeAvecLesIdentifiantsValides()
    {
        var candidature = Entites.Candidature.Creer(CandidatId, AnnonceId, "Lettre de motivation.");

        candidature.Id.Should().NotBe(Guid.Empty);
        candidature.CandidatId.Should().Be(CandidatId);
        candidature.AnnonceId.Should().Be(AnnonceId);
        candidature.LettreMotivation.Should().Be("Lettre de motivation.");
    }

    [Fact]
    public void UneCandidatureAUneDateDePostulationRenseignee()
    {
        var avant = DateTime.UtcNow;
        var candidature = Entites.Candidature.Creer(CandidatId, AnnonceId, null);

        candidature.DatePostulation.Should().BeOnOrAfter(avant);
    }

    [Fact]
    public void UneCandidatureSansLettreDeMotivationEstAcceptee()
    {
        var candidature = Entites.Candidature.Creer(CandidatId, AnnonceId, null);

        candidature.LettreMotivation.Should().BeNull();
    }

    [Fact]
    public void CreerAvecCandidatIdVideLeveArgumentException()
    {
        var creer = () => Entites.Candidature.Creer(Guid.Empty, AnnonceId, null);

        creer.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreerAvecAnnonceIdVideLeveArgumentException()
    {
        var creer = () => Entites.Candidature.Creer(CandidatId, Guid.Empty, null);

        creer.Should().Throw<ArgumentException>();
    }
}
