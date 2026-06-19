using CatalogueEmploi.Application.Features.SupprimerAnnonce;
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

namespace CatalogueEmploi.Application.Tests.SupprimerAnnonce;

public sealed class SupprimerAnnonceHandlerTests
{
    private readonly Mock<IAnnonceEmploiRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly SupprimerAnnonceHandler _handler;

    private static readonly Guid AdminId = Guid.NewGuid();
    private static readonly Guid AnnonceId = Guid.NewGuid();

    public SupprimerAnnonceHandlerTests()
    {
        _handler = new SupprimerAnnonceHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.SupprimerAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var supprimer = () => _handler.Handle(
            new SupprimerAnnonceCommand { AdministrateurId = AdminId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await supprimer.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task SiLAnnonceFIntrouvableLHandlerLeveAnnonceNonTrouveeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.SupprimerAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AnnonceEmploi?)null);

        var supprimer = () => _handler.Handle(
            new SupprimerAnnonceCommand { AdministrateurId = AdminId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await supprimer.Should().ThrowAsync<AnnonceNonTrouveeException>();
    }

    [Fact]
    public async Task UnAdministrateurPeutSupprimerUneAnnonce()
    {
        var annonce = AnnonceEmploi.Publier(Guid.NewGuid(), "Dev .NET", "Mission.",
            TypeContrat.CDI, DomaineMetier.Creer("Cloud Azure"));
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.SupprimerAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(annonce);

        await _handler.Handle(
            new SupprimerAnnonceCommand { AdministrateurId = AdminId, AnnonceId = AnnonceId },
            CancellationToken.None);

        _repository.Verify(r => r.MettreAJourAsync(
            It.IsAny<AnnonceEmploi>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
