using AppelOffreFreelance.Application.Features.ModererAppelOffre;
using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Exceptions;
using AppelOffreFreelance.Domaine.ObjetsValeur;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace AppelOffreFreelance.Application.Tests.ModererAppelOffre;

public sealed class ModererAppelOffreHandlerTests
{
    private readonly Mock<IAppelOffreRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly ModererAppelOffreHandler _handler;

    private static readonly Guid AdminId = Guid.NewGuid();
    private static readonly Guid AppelOffreId = Guid.NewGuid();

    public ModererAppelOffreHandlerTests()
    {
        _handler = new ModererAppelOffreHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.ModererAppelOffre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var moderer = () => _handler.Handle(
            new ModererAppelOffreCommand { AdministrateurId = AdminId, AppelOffreId = AppelOffreId },
            CancellationToken.None);

        await moderer.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task SiLAppelOffreEstIntrouvableLHandlerLeveAppelOffreNonTrouveException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.ModererAppelOffre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParIdAsync(AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AppelOffre?)null);

        var moderer = () => _handler.Handle(
            new ModererAppelOffreCommand { AdministrateurId = AdminId, AppelOffreId = AppelOffreId },
            CancellationToken.None);

        await moderer.Should().ThrowAsync<AppelOffreNonTrouveException>();
    }

    [Fact]
    public async Task UnAdministrateurPeutModererUnAppelOffre()
    {
        var appelOffre = AppelOffre.Publier(Guid.NewGuid(), "Mission", "Desc.",
            DomaineMetier.Creer("DevOps"), BaremeTJM.Creer(400, 700), DateTime.UtcNow.AddDays(30));
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.ModererAppelOffre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParIdAsync(AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appelOffre);

        await _handler.Handle(
            new ModererAppelOffreCommand { AdministrateurId = AdminId, AppelOffreId = AppelOffreId },
            CancellationToken.None);

        _repository.Verify(r => r.MettreAJourAsync(
            It.IsAny<AppelOffre>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
