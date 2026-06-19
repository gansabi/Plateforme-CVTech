using FluentAssertions;
using GestionIdentite.Application.Features.CreerCompteAdministrateur;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Exceptions;
using Moq;
using Xunit;

namespace GestionIdentite.Application.Tests.CreerCompteAdministrateur;

public sealed class CreerCompteAdministrateurHandlerTests
{
    private const string EmailValide = "admin@exemple.fr";
    private const string NomCompletValide = "Alice Martin";
    private const string MotDePasseValide = "S3cur3Admin!";

    private readonly Mock<ICompteAdministrateurRepository> _repository = new();
    private readonly Mock<IHasheurMotDePasse> _hasheur = new();
    private readonly CreerCompteAdministrateurHandler _handler;

    public CreerCompteAdministrateurHandlerTests()
    {
        _hasheur.Setup(h => h.Hacher(It.IsAny<string>())).Returns("hash-admin-fictif");
        _handler = new CreerCompteAdministrateurHandler(_repository.Object, _hasheur.Object);
    }

    // -----------------------------------------------------------------------
    // Chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public async Task UnNouveauCompteAdminEstSauvegardeQuandLesInformationsSontValides()
    {
        _repository
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreerCompteAdministrateurCommand
        {
            Email = EmailValide,
            NomComplet = NomCompletValide,
            MotDePasse = MotDePasseValide
        };

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(
            r => r.SauvegarderAsync(It.IsAny<CompteAdministrateur>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task LeHandlerRetourneLeIdentifiantDuNouveauCompteAdmin()
    {
        _repository
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreerCompteAdministrateurCommand
        {
            Email = EmailValide,
            NomComplet = NomCompletValide,
            MotDePasse = MotDePasseValide
        };

        var reponse = await _handler.Handle(command, CancellationToken.None);

        reponse.CompteId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task LeMotDePasseAdminEstHacheLorsDeLaCreation()
    {
        CompteAdministrateur? compteSauvegarde = null;

        _repository
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repository
            .Setup(r => r.SauvegarderAsync(It.IsAny<CompteAdministrateur>(), It.IsAny<CancellationToken>()))
            .Callback<CompteAdministrateur, CancellationToken>((c, _) => compteSauvegarde = c)
            .Returns(Task.CompletedTask);

        var command = new CreerCompteAdministrateurCommand
        {
            Email = EmailValide,
            NomComplet = NomCompletValide,
            MotDePasse = MotDePasseValide
        };

        await _handler.Handle(command, CancellationToken.None);

        compteSauvegarde!.MotDePasseHache.Should().Be("hash-admin-fictif");
    }

    // -----------------------------------------------------------------------
    // Unicité de l'email
    // -----------------------------------------------------------------------

    [Fact]
    public async Task SiLEmailEstDejaUtiliseLHandlerLeveEmailDejaUtiliseException()
    {
        _repository
            .Setup(r => r.ExisteAvecEmailAsync(EmailValide, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreerCompteAdministrateurCommand
        {
            Email = EmailValide,
            NomComplet = NomCompletValide,
            MotDePasse = MotDePasseValide
        };

        var creer = () => _handler.Handle(command, CancellationToken.None);

        await creer.Should().ThrowAsync<EmailDejaUtiliseException>();
    }

    [Fact]
    public async Task SiLEmailEstDejaUtiliseLCompteAdminNEstPasSauvegarde()
    {
        _repository
            .Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreerCompteAdministrateurCommand
        {
            Email = EmailValide,
            NomComplet = NomCompletValide,
            MotDePasse = MotDePasseValide
        };

        await ((Func<Task>)(() => _handler.Handle(command, CancellationToken.None)))
            .Should().ThrowAsync<EmailDejaUtiliseException>();

        _repository.Verify(
            r => r.SauvegarderAsync(It.IsAny<CompteAdministrateur>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
