using AppelOffreFreelance.Domaine.Entites;
using FluentAssertions;
using Xunit;

namespace AppelOffreFreelance.Domaine.Tests.PropositionFreelance;

public sealed class PropositionFreelanceTests
{
    private static readonly Guid CandidatId = Guid.NewGuid();
    private static readonly Guid AppelOffreId = Guid.NewGuid();

    [Fact]
    public void UnePropositionEstCreeeAvecLesInformationsValides()
    {
        var proposition = Entites.PropositionFreelance.Creer(
            CandidatId, AppelOffreId, 600, 20, "Agile Scrum");

        proposition.Id.Should().NotBe(Guid.Empty);
        proposition.CandidatId.Should().Be(CandidatId);
        proposition.AppelOffreId.Should().Be(AppelOffreId);
        proposition.TarifJournalier.Should().Be(600);
        proposition.DureeJours.Should().Be(20);
        proposition.Methodologie.Should().Be("Agile Scrum");
    }

    [Fact]
    public void UnePropositionAUneDateDeSoumissionRenseignee()
    {
        var avant = DateTime.UtcNow;
        var proposition = Entites.PropositionFreelance.Creer(
            CandidatId, AppelOffreId, 500, 10, "Kanban");

        proposition.DateSoumission.Should().BeOnOrAfter(avant);
    }

    [Fact]
    public void CreerAvecCandidatIdVideLeveArgumentException()
    {
        var creer = () => Entites.PropositionFreelance.Creer(
            Guid.Empty, AppelOffreId, 500, 10, "Scrum");

        creer.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreerAvecAppelOffreIdVideLeveArgumentException()
    {
        var creer = () => Entites.PropositionFreelance.Creer(
            CandidatId, Guid.Empty, 500, 10, "Scrum");

        creer.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreerAvecTarifNegatifLeveArgumentException()
    {
        var creer = () => Entites.PropositionFreelance.Creer(
            CandidatId, AppelOffreId, 0, 10, "Scrum");

        creer.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreerAvecDureeNulleLeveArgumentException()
    {
        var creer = () => Entites.PropositionFreelance.Creer(
            CandidatId, AppelOffreId, 500, 0, "Scrum");

        creer.Should().Throw<ArgumentException>();
    }
}
