using CatalogueEmploi.Application.Features.PublierAnnonce;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Evenements;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace CatalogueEmploi.Application.Tests.PublierAnnonce;

public sealed class PublierAnnonceHandlerTests
{
    private readonly Mock<IAnnonceEmploiRepository> _repository = new();
    private readonly Mock<IBusEvenements> _bus = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly PublierAnnonceHandler _handler;

    private static readonly Guid EntrepriseId = Guid.NewGuid();

    private static PublierAnnonceCommand CommandeValide() => new()
    {
        UtilisateurId = EntrepriseId,
        Titre = "Développeur .NET Senior",
        Description = "Mission .NET 10 en architecture modulaire.",
        TypeContrat = "CDI",
        DomaineMetier = "Cloud Azure"
    };

    public PublierAnnonceHandlerTests()
    {
        _handler = new PublierAnnonceHandler(_repository.Object, _bus.Object, _verificateur.Object);
    }

    // -----------------------------------------------------------------------
    // Vérification permissions (FIRST)
    // -----------------------------------------------------------------------

    [Fact]
    public async Task SiLaPermissionEstRefuseeLeHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var publier = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await publier.Should().ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeAucuneAnnonceNEstSauvegardee()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await ((Func<Task>)(() => _handler.Handle(CommandeValide(), CancellationToken.None)))
            .Should().ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();

        _repository.Verify(r => r.SauvegarderAsync(
            It.IsAny<AnnonceEmploi>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // -----------------------------------------------------------------------
    // Chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public async Task UneAnnonceEstSauvegardeeQuandLesInformationsSontValides()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(CommandeValide(), CancellationToken.None);

        _repository.Verify(r => r.SauvegarderAsync(
            It.IsAny<AnnonceEmploi>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LeHandlerRetourneLeIdentifiantDeLaNouvelleAnnonce()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var reponse = await _handler.Handle(CommandeValide(), CancellationToken.None);

        reponse.AnnonceId.Should().NotBe(Guid.Empty);
    }

    // -----------------------------------------------------------------------
    // Événement métier
    // -----------------------------------------------------------------------

    [Fact]
    public async Task UneAnnoncePublieeGenereLevenementAnnoncePubliee()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(CommandeValide(), CancellationToken.None);

        _bus.Verify(b => b.PublierAsync(
            It.IsAny<AnnoncePubliee>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LEvenementAnnoncePublieeContientLesBonnesDonnees()
    {
        AnnoncePubliee? evenementCapture = null;
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.PublierAnnonce, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _bus.Setup(b => b.PublierAsync(It.IsAny<AnnoncePubliee>(), It.IsAny<CancellationToken>()))
            .Callback<AnnoncePubliee, CancellationToken>((e, _) => evenementCapture = e)
            .Returns(Task.CompletedTask);

        await _handler.Handle(CommandeValide(), CancellationToken.None);

        evenementCapture.Should().NotBeNull();
        evenementCapture!.EntrepriseId.Should().Be(EntrepriseId);
        evenementCapture.Titre.Should().Be("Développeur .NET Senior");
        evenementCapture.DomaineMetier.Should().Be("Cloud Azure");
    }
}
