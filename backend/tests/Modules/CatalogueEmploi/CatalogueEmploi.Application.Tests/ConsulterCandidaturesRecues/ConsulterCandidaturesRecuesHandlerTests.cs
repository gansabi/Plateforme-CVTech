using CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace CatalogueEmploi.Application.Tests.ConsulterCandidaturesRecues;

public sealed class ConsulterCandidaturesRecuesHandlerTests
{
    private readonly Mock<ICandidatureRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly ConsulterCandidaturesRecuesHandler _handler;

    private static readonly Guid EntrepriseId = Guid.NewGuid();
    private static readonly Guid AnnonceId = Guid.NewGuid();

    public ConsulterCandidaturesRecuesHandlerTests()
    {
        _handler = new ConsulterCandidaturesRecuesHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.ConsulterCandidaturesRecues, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var consulter = () => _handler.Handle(
            new ConsulterCandidaturesRecuesQuery { UtilisateurId = EntrepriseId, AnnonceId = AnnonceId },
            CancellationToken.None);

        await consulter.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UneEntreprisePeutConsulterLesCandidaturesDeSesPropresAnnonces()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.ConsulterCandidaturesRecues, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.ListerParAnnonceIdAsync(AnnonceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([Candidature.Creer(Guid.NewGuid(), AnnonceId, null)]);

        var reponse = await _handler.Handle(
            new ConsulterCandidaturesRecuesQuery { UtilisateurId = EntrepriseId, AnnonceId = AnnonceId },
            CancellationToken.None);

        reponse.Candidatures.Should().HaveCount(1);
    }
}
