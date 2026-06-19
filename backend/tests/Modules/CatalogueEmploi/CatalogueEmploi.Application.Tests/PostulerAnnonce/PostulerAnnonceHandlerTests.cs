using CatalogueEmploi.Application.Features.PostulerAnnonce;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Enums;
using CatalogueEmploi.Domaine.Exceptions;
using CatalogueEmploi.Domaine.ObjetsValeur;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace CatalogueEmploi.Application.Tests.PostulerAnnonce;

public sealed class PostulerAnnonceHandlerTests
{
    private readonly Mock<ICandidatureRepository> _candidatureRepo = new();
    private readonly Mock<IAnnonceEmploiRepository> _annonceRepo = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly PostulerAnnonceHandler _handler;

    private static readonly Guid CandidatId = Guid.NewGuid();
    private static readonly Guid AnnonceId = Guid.NewGuid();

    private static AnnonceEmploi AnnonceActive() =>
        AnnonceEmploi.Publier(Guid.NewGuid(), "Dev .NET", "Mission.",
            TypeContrat.CDI, DomaineMetier.Creer("Cloud Azure"));

    public PostulerAnnonceHandlerTests()
    {
        _handler = new PostulerAnnonceHandler(
            _candidatureRepo.Object, _annonceRepo.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.PostulerAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var postuler = () => _handler.Handle(
            new PostulerAnnonceCommand { CandidatId = CandidatId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await postuler.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UnCandidatPeutPostulerAUneAnnonce()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.PostulerAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _annonceRepo.Setup(r => r.TrouverParIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AnnonceActive());
        _candidatureRepo.Setup(r => r.ExisteDejaAsync(
                CandidatId, AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _handler.Handle(
            new PostulerAnnonceCommand { CandidatId = CandidatId, AnnonceId = AnnonceId },
            CancellationToken.None);

        _candidatureRepo.Verify(r => r.SauvegarderAsync(
            It.IsAny<Candidature>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SiLAnnonceFIntrouvableLHandlerLeveAnnonceNonTrouveeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.PostulerAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _annonceRepo.Setup(r => r.TrouverParIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AnnonceEmploi?)null);

        var postuler = () => _handler.Handle(
            new PostulerAnnonceCommand { CandidatId = CandidatId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await postuler.Should().ThrowAsync<AnnonceNonTrouveeException>();
    }

    [Fact]
    public async Task SiLeCandidatADejaPostuleLHandlerLeveCandidatureDejaExistanteException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.PostulerAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _annonceRepo.Setup(r => r.TrouverParIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AnnonceActive());
        _candidatureRepo.Setup(r => r.ExisteDejaAsync(
                CandidatId, AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var postuler = () => _handler.Handle(
            new PostulerAnnonceCommand { CandidatId = CandidatId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await postuler.Should().ThrowAsync<CandidatureDejaExistanteException>();
    }
}
