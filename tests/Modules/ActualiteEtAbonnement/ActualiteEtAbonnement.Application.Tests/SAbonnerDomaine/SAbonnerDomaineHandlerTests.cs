using ActualiteEtAbonnement.Application.Features.SAbonnerDomaine;
using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Exceptions;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace ActualiteEtAbonnement.Application.Tests.SAbonnerDomaine;

public sealed class SAbonnerDomaineHandlerTests
{
    private readonly Mock<IAbonnementRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly SAbonnerDomaineHandler _handler;

    private static readonly Guid UtilisateurId = Guid.NewGuid();

    private static SAbonnerDomaineCommand CommandeValide() => new()
    {
        UtilisateurId = UtilisateurId,
        DomaineMetier = "Cloud Azure"
    };

    public SAbonnerDomaineHandlerTests()
    {
        _handler = new SAbonnerDomaineHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                UtilisateurId, Permission.SAbonnerDomaine, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var sabonner = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await sabonner.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UnUtilisateurPeutSAbonnerAUnDomaine()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                UtilisateurId, Permission.SAbonnerDomaine, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.ExisteDejaAsync(UtilisateurId, "Cloud Azure", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _handler.Handle(CommandeValide(), CancellationToken.None);

        _repository.Verify(r => r.SauvegarderAsync(
            It.IsAny<Abonnement>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SiLAbonnementExisteDéjaLHandlerLeveAbonnementDejaExistantException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                UtilisateurId, Permission.SAbonnerDomaine, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.ExisteDejaAsync(UtilisateurId, "Cloud Azure", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sabonner = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await sabonner.Should().ThrowAsync<AbonnementDejaExistantException>();
    }
}
