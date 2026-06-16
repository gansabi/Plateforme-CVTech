using CatalogueEmploi.Application.Features.CreerCurriculumVitae;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace CatalogueEmploi.Application.Tests.CreerCurriculumVitae;

public sealed class CreerCurriculumVitaeHandlerTests
{
    private readonly Mock<ICurriculumVitaeRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly CreerCurriculumVitaeHandler _handler;

    private static readonly Guid CandidatId = Guid.NewGuid();

    private static CreerCurriculumVitaeCommand CommandeValide() => new()
    {
        CandidatId = CandidatId,
        Titre = "Développeur .NET Senior",
        Resume = "Cinq ans d'expérience.",
        CompetencesPrincipales = ["C#", ".NET"]
    };

    public CreerCurriculumVitaeHandlerTests()
    {
        _handler = new CreerCurriculumVitaeHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.CreerCurriculumVitae, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var creer = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await creer.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UnCandidatPeutCreerSonCurriculumVitae()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.CreerCurriculumVitae, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var reponse = await _handler.Handle(CommandeValide(), CancellationToken.None);

        reponse.CurriculumVitaeId.Should().NotBe(Guid.Empty);
        _repository.Verify(r => r.SauvegarderAsync(
            It.IsAny<CurriculumVitae>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
