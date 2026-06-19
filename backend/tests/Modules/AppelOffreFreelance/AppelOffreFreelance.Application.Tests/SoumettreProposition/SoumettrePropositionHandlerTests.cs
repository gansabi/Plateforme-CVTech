using AppelOffreFreelance.Application.Features.SoumettreProposition;
using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Exceptions;
using AppelOffreFreelance.Domaine.ObjetsValeur;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace AppelOffreFreelance.Application.Tests.SoumettreProposition;

public sealed class SoumettrePropositionHandlerTests
{
    private readonly Mock<IPropositionFreelanceRepository> _propositionRepo = new();
    private readonly Mock<IAppelOffreRepository> _appelOffreRepo = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly SoumettrePropositionHandler _handler;

    private static readonly Guid CandidatId = Guid.NewGuid();
    private static readonly Guid AppelOffreId = Guid.NewGuid();

    private static AppelOffre AppelOffreOuvert() =>
        AppelOffre.Publier(Guid.NewGuid(), "Mission", "Description.",
            DomaineMetier.Creer("DevOps"), BaremeTJM.Creer(400, 700), DateTime.UtcNow.AddDays(30));

    public SoumettrePropositionHandlerTests()
    {
        _handler = new SoumettrePropositionHandler(
            _propositionRepo.Object, _appelOffreRepo.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.SoumettrePropositon, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var soumettre = () => _handler.Handle(
            new SoumettrePropositionCommand { CandidatId = CandidatId, AppelOffreId = AppelOffreId, TarifJournalier = 600, DureeJours = 20, Methodologie = "Scrum" },
            CancellationToken.None);

        await soumettre.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UnCandidatPeutSoumettreUneProposition()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.SoumettrePropositon, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _appelOffreRepo.Setup(r => r.TrouverParIdAsync(AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AppelOffreOuvert());
        _propositionRepo.Setup(r => r.ExisteDejaAsync(CandidatId, AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _handler.Handle(
            new SoumettrePropositionCommand { CandidatId = CandidatId, AppelOffreId = AppelOffreId, TarifJournalier = 600, DureeJours = 20, Methodologie = "Scrum" },
            CancellationToken.None);

        _propositionRepo.Verify(r => r.SauvegarderAsync(
            It.IsAny<PropositionFreelance>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SiLAppelOffreEstIntrouvableLHandlerLeveAppelOffreNonTrouveException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.SoumettrePropositon, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _appelOffreRepo.Setup(r => r.TrouverParIdAsync(AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AppelOffre?)null);

        var soumettre = () => _handler.Handle(
            new SoumettrePropositionCommand { CandidatId = CandidatId, AppelOffreId = AppelOffreId, TarifJournalier = 600, DureeJours = 20, Methodologie = "Scrum" },
            CancellationToken.None);

        await soumettre.Should().ThrowAsync<AppelOffreNonTrouveException>();
    }

    [Fact]
    public async Task SiLeCandidatADejaSoumisLHandlerLevePropositionDejaExistanteException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.SoumettrePropositon, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _appelOffreRepo.Setup(r => r.TrouverParIdAsync(AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AppelOffreOuvert());
        _propositionRepo.Setup(r => r.ExisteDejaAsync(CandidatId, AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var soumettre = () => _handler.Handle(
            new SoumettrePropositionCommand { CandidatId = CandidatId, AppelOffreId = AppelOffreId, TarifJournalier = 600, DureeJours = 20, Methodologie = "Scrum" },
            CancellationToken.None);

        await soumettre.Should().ThrowAsync<PropositionDejaExistanteException>();
    }
}
