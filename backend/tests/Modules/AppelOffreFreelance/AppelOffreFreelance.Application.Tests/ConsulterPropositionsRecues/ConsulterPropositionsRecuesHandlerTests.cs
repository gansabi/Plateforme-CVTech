using AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;
using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using FluentAssertions;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using Moq;
using Xunit;

namespace AppelOffreFreelance.Application.Tests.ConsulterPropositionsRecues;

public sealed class ConsulterPropositionsRecuesHandlerTests
{
    private readonly Mock<IPropositionFreelanceRepository> _repository = new();
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly ConsulterPropositionsRecuesHandler _handler;

    private static readonly Guid EntrepriseId = Guid.NewGuid();
    private static readonly Guid AppelOffreId = Guid.NewGuid();

    public ConsulterPropositionsRecuesHandlerTests()
    {
        _handler = new ConsulterPropositionsRecuesHandler(_repository.Object, _verificateur.Object);
    }

    [Fact]
    public async Task SiLaPermissionEstRefuseeLHandlerLevePermissionRefuseeException()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.ConsulterPropositionsRecues, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var consulter = () => _handler.Handle(
            new ConsulterPropositionsRecuesQuery { UtilisateurId = EntrepriseId, AppelOffreId = AppelOffreId },
            CancellationToken.None);

        await consulter.Should()
            .ThrowAsync<GestionIdentite.Domaine.Exceptions.PermissionRefuseeException>();
    }

    [Fact]
    public async Task UneEntreprisePeutConsulterLesPropositionsRecues()
    {
        _verificateur.Setup(v => v.PossedePermissionAsync(
                EntrepriseId, Permission.ConsulterPropositionsRecues, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(r => r.ListerParAppelOffreIdAsync(AppelOffreId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([PropositionFreelance.Creer(Guid.NewGuid(), AppelOffreId, 600, 20, "Agile")]);

        var reponse = await _handler.Handle(
            new ConsulterPropositionsRecuesQuery { UtilisateurId = EntrepriseId, AppelOffreId = AppelOffreId },
            CancellationToken.None);

        reponse.Propositions.Should().HaveCount(1);
    }
}
