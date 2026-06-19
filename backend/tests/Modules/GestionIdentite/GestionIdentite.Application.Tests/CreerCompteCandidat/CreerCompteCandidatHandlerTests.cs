using FluentAssertions;
using GestionIdentite.Application.Features.CreerCompteCandidat;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Exceptions;
using Moq;
using Xunit;

namespace GestionIdentite.Application.Tests.CreerCompteCandidat;

public sealed class CreerCompteCandidatHandlerTests
{
    private const string EmailValide = "candidat@exemple.fr";
    private const string EmailExistant = "existant@exemple.fr";
    private const string MotDePasseValide = "S3cr3t!Ok";

    private readonly Mock<ICompteRepository> _repositoireCompte = new();
    private readonly Mock<IHasheurMotDePasse> _hasheur = new();
    private readonly CreerCompteCandidatHandler _handler;

    public CreerCompteCandidatHandlerTests()
    {
        _hasheur.Setup(h => h.Hacher(It.IsAny<string>())).Returns("hash-fictif");
        _handler = new CreerCompteCandidatHandler(_repositoireCompte.Object, _hasheur.Object);
    }

    // -----------------------------------------------------------------------
    // Chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public async Task UnNouveauCompteEstSauvegardeQuandLesInformationsSontValides()
    {
        _repositoireCompte
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreerCompteCandidatCommand { Email = EmailValide, MotDePasse = MotDePasseValide };

        await _handler.Handle(command, CancellationToken.None);

        _repositoireCompte.Verify(
            r => r.SauvegarderAsync(It.IsAny<CompteCandidat>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task LeHandlerRetourneLeIdentifiantDuNouveauCompte()
    {
        _repositoireCompte
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreerCompteCandidatCommand { Email = EmailValide, MotDePasse = MotDePasseValide };

        var reponse = await _handler.Handle(command, CancellationToken.None);

        reponse.CompteId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task LeHandlerCreeLeCompteAvecLeRoleCandidat()
    {
        CompteCandidat? compteSauvegarde = null;

        _repositoireCompte
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repositoireCompte
            .Setup(r => r.SauvegarderAsync(It.IsAny<CompteCandidat>(), It.IsAny<CancellationToken>()))
            .Callback<CompteCandidat, CancellationToken>((c, _) => compteSauvegarde = c)
            .Returns(Task.CompletedTask);

        var command = new CreerCompteCandidatCommand { Email = EmailValide, MotDePasse = MotDePasseValide };

        await _handler.Handle(command, CancellationToken.None);

        compteSauvegarde.Should().NotBeNull();
        compteSauvegarde!.Role.Should().Be(GestionIdentite.Domaine.Enums.Role.Candidat);
    }

    // -----------------------------------------------------------------------
    // Unicité de l'email
    // -----------------------------------------------------------------------

    [Fact]
    public async Task SiLEmailEstDejaUtiliseLHandlerLeveUneExceptionMetier()
    {
        _repositoireCompte
            .Setup(r => r.ExisteAvecEmailAsync(EmailExistant, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreerCompteCandidatCommand { Email = EmailExistant, MotDePasse = MotDePasseValide };

        var creer = () => _handler.Handle(command, CancellationToken.None);

        await creer.Should().ThrowAsync<EmailDejaUtiliseException>();
    }

    [Fact]
    public async Task SiLEmailEstDejaUtiliseLCompteNEstPasSauvegarde()
    {
        _repositoireCompte
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreerCompteCandidatCommand { Email = EmailExistant, MotDePasse = MotDePasseValide };

        await ((Func<Task>)(() => _handler.Handle(command, CancellationToken.None)))
            .Should().ThrowAsync<EmailDejaUtiliseException>();

        _repositoireCompte.Verify(
            r => r.SauvegarderAsync(It.IsAny<CompteCandidat>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // -----------------------------------------------------------------------
    // Hachage du mot de passe
    // -----------------------------------------------------------------------

    [Fact]
    public async Task LeMotDePasseEstHacheLorsDeLaCreationDuCompte()
    {
        CompteCandidat? compteSauvegarde = null;

        _repositoireCompte
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repositoireCompte
            .Setup(r => r.SauvegarderAsync(It.IsAny<CompteCandidat>(), It.IsAny<CancellationToken>()))
            .Callback<CompteCandidat, CancellationToken>((c, _) => compteSauvegarde = c)
            .Returns(Task.CompletedTask);

        var command = new CreerCompteCandidatCommand { Email = EmailValide, MotDePasse = MotDePasseValide };

        await _handler.Handle(command, CancellationToken.None);

        compteSauvegarde!.MotDePasseHache.Should().Be("hash-fictif");
    }

    // -----------------------------------------------------------------------
    // Intégrité — le Handler vérifie l'email AVANT d'accéder au domaine
    // -----------------------------------------------------------------------

    [Fact]
    public async Task LeHandlerVerifieExistenceEmailAvantDeCreerLeCompte()
    {
        var ordreAppels = new List<string>();

        _repositoireCompte
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback(() => ordreAppels.Add("ExisteAvecEmailAsync"))
            .ReturnsAsync(false);
        _repositoireCompte
            .Setup(r => r.SauvegarderAsync(It.IsAny<CompteCandidat>(), It.IsAny<CancellationToken>()))
            .Callback(() => ordreAppels.Add("SauvegarderAsync"))
            .Returns(Task.CompletedTask);

        var command = new CreerCompteCandidatCommand { Email = EmailValide, MotDePasse = MotDePasseValide };

        await _handler.Handle(command, CancellationToken.None);

        ordreAppels.Should().ContainInOrder("ExisteAvecEmailAsync", "SauvegarderAsync");
    }
}
