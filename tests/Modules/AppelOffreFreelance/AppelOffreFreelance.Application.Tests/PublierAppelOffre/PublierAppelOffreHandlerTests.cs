using AppelOffreFreelance.Application.Features.PublierAppelOffre;
using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Evenements;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace AppelOffreFreelance.Application.Tests.PublierAppelOffre;

public sealed class PublierAppelOffreHandlerTests
{
    private readonly Mock<IAppelOffreRepository> _repository = new();
    private readonly Mock<IBusEvenements> _bus = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly PublierAppelOffreHandler _handler;

    private static readonly Guid EntrepriseId = Guid.NewGuid();

    private static PublierAppelOffreCommand CommandeValide() => new()
    {
        UtilisateurId = EntrepriseId,
        Titre = "Mission Architecture Cloud",
        Description = "Conception infrastructure Azure.",
        DomaineMetier = "Cloud Azure",
        TjmMinimum = 500,
        TjmMaximum = 800,
        DateLimite = DateTime.UtcNow.AddDays(30)
    };

    public PublierAppelOffreHandlerTests()
    {
        _handler = new PublierAppelOffreHandler(_repository.Object, _bus.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAppelOffre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var publier = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await publier.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UnAppelOffreEstSauvegardéQuandLesInformationsSontValides()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAppelOffre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(CommandeValide(), CancellationToken.None);

        _repository.Verify(r => r.SauvegarderAsync(
            It.IsAny<AppelOffre>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UnAppelOffrePublieGenereLEvenementAppelOffrePublie()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAppelOffre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(CommandeValide(), CancellationToken.None);

        _bus.Verify(b => b.PublierAsync(
            It.IsAny<AppelOffrePublie>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LeHandlerRetourneLIdentifiantDuNouvelAppelOffre()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAppelOffre, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var reponse = await _handler.Handle(CommandeValide(), CancellationToken.None);

        reponse.AppelOffreId.Should().NotBe(Guid.Empty);
    }
}
