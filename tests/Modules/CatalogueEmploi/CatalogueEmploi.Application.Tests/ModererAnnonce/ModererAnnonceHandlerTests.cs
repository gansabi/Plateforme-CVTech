using CatalogueEmploi.Application.Features.ModererAnnonce;
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

namespace CatalogueEmploi.Application.Tests.ModererAnnonce;

public sealed class ModererAnnonceHandlerTests
{
    private readonly Mock<IAnnonceEmploiRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly ModererAnnonceHandler _handler;

    private static readonly Guid AdminId = Guid.NewGuid();
    private static readonly Guid AnnonceId = Guid.NewGuid();

    public ModererAnnonceHandlerTests()
    {
        _handler = new ModererAnnonceHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.ModererAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var moderer = () => _handler.Handle(
            new ModererAnnonceCommand { AdministrateurId = AdminId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await moderer.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task SiLAnnonceFIntrouvableLHandlerLeveAnnonceNonTrouveeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.ModererAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AnnonceEmploi?)null);

        var moderer = () => _handler.Handle(
            new ModererAnnonceCommand { AdministrateurId = AdminId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await moderer.Should().ThrowAsync<AnnonceNonTrouveeException>();
    }

    [Fact]
    public async Task UnAdministrateurPeutModererUneAnnonce()
    {
        var annonce = AnnonceEmploi.Publier(Guid.NewGuid(), "Dev .NET", "Mission.",
            TypeContrat.CDI, DomaineMetier.Creer("Cloud Azure"));
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.ModererAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(annonce);

        await _handler.Handle(
            new ModererAnnonceCommand { AdministrateurId = AdminId, AnnonceId = AnnonceId },
            CancellationToken.None);

        _repository.Verify(r => r.MettreAJourAsync(
            It.IsAny<AnnonceEmploi>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLeRepositoryNEstJamaisAppele()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.ModererAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await ((Func<Task>)(() => _handler.Handle(
            new ModererAnnonceCommand { AdministrateurId = AdminId, AnnonceId = AnnonceId },
            CancellationToken.None))).Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();

        _repository.Verify(r => r.TrouverParIdAsync(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
