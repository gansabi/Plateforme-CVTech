using ActualiteEtAbonnement.Application.Features.PublierArticle;
using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace ActualiteEtAbonnement.Application.Tests.PublierArticle;

public sealed class PublierArticleHandlerTests
{
    private readonly Mock<IArticleActualiteRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly PublierArticleHandler _handler;

    private static readonly Guid AdminId = Guid.NewGuid();

    private static PublierArticleCommand CommandeValide() => new()
    {
        AuteurId = AdminId,
        Titre = "Les tendances Cloud 2026",
        Contenu = "Le serverless continue de croître.",
        DomaineMetier = "Cloud Azure"
    };

    public PublierArticleHandlerTests()
    {
        _handler = new PublierArticleHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.PublierArticleActualite, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var publier = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await publier.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UnAdministrateurPeutPublierUnArticle()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                AdminId, Permission.PublierArticleActualite, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var reponse = await _handler.Handle(CommandeValide(), CancellationToken.None);

        reponse.ArticleId.Should().NotBe(Guid.Empty);
        _repository.Verify(r => r.SauvegarderAsync(
            It.IsAny<ArticleActualite>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
