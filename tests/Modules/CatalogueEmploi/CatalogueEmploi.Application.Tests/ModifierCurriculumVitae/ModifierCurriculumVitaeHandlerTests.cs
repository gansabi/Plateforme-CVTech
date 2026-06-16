using CatalogueEmploi.Application.Features.ModifierCurriculumVitae;
using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.Exceptions;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace CatalogueEmploi.Application.Tests.ModifierCurriculumVitae;

public sealed class ModifierCurriculumVitaeHandlerTests
{
    private readonly Mock<ICurriculumVitaeRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly ModifierCurriculumVitaeHandler _handler;

    private static readonly Guid CandidatId = Guid.NewGuid();

    private static ModifierCurriculumVitaeCommand CommandeValide() => new()
    {
        CandidatId = CandidatId,
        Titre = "Architecte Cloud",
        Resume = "Expert Azure.",
        CompetencesPrincipales = ["Azure", "Terraform"]
    };

    public ModifierCurriculumVitaeHandlerTests()
    {
        _handler = new ModifierCurriculumVitaeHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.ModifierCurriculumVitae, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var modifier = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await modifier.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task SiLeCVNExistePasLHandlerLeveCurriculumVitaeNonTrouveException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.ModifierCurriculumVitae, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParCandidatIdAsync(CandidatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CurriculumVitae?)null);

        var modifier = () => _handler.Handle(CommandeValide(), CancellationToken.None);

        await modifier.Should().ThrowAsync<CurriculumVitaeNonTrouveException>();
    }

    [Fact]
    public async Task UnCandidatPeutModifierSonCurriculumVitae()
    {
        var cv = CurriculumVitae.Creer(CandidatId, "Ancien titre", "Ancien résumé.", []);
        _verificateur.Setup(v => v.PossedePermissionAsync(
                CandidatId, Permission.ModifierCurriculumVitae, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.TrouverParCandidatIdAsync(CandidatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cv);

        await _handler.Handle(CommandeValide(), CancellationToken.None);

        _repository.Verify(r => r.MettreAJourAsync(
            It.IsAny<CurriculumVitae>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
