using FluentAssertions;
using GestionIdentite.Application.Features.CreerCompteEntreprise;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Exceptions;
using Moq;
using Xunit;

namespace GestionIdentite.Application.Tests.CreerCompteEntreprise;

public sealed class CreerCompteEntrepriseHandlerTests
{
    private readonly Mock<ICompteEntrepriseRepository> _repo = new();
    private readonly Mock<IHasheurMotDePasse> _hasheur = new();
    private readonly CreerCompteEntrepriseHandler _handler;

    private static readonly CreerCompteEntrepriseCommand CommandeValide = new()
    {
        Email = "entreprise@exemple.fr",
        MotDePasse = "S3cr3t!Ok",
        NomEntreprise = "TechCorp SAS"
    };

    public CreerCompteEntrepriseHandlerTests()
    {
        _hasheur.Setup(h => h.Hacher(It.IsAny<string>())).Returns("hash_bcrypt");
        _handler = new CreerCompteEntrepriseHandler(_repo.Object, _hasheur.Object);
    }

    [Fact]
    public async Task LaCreationDUnCompteEntrepriseAvecDesParametresValidesRetourneUnId()
    {
        _repo.Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(false);

        var reponse = await _handler.Handle(CommandeValide, CancellationToken.None);

        reponse.CompteId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task LaCreationAvecUnEmailDejaUtiliseLeveUneEmailDejaUtiliseException()
    {
        _repo.Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(true);

        var creer = () => _handler.Handle(CommandeValide, CancellationToken.None);

        await creer.Should().ThrowAsync<EmailDejaUtiliseException>();
    }

    [Fact]
    public async Task LaCreationSauvegardeLeCompteEntreprise()
    {
        _repo.Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(false);

        await _handler.Handle(CommandeValide, CancellationToken.None);

        _repo.Verify(r => r.SauvegarderAsync(It.IsAny<CompteEntreprise>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LaCreationHacheLeMoteDePasseAvantPersistance()
    {
        _repo.Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(false);

        await _handler.Handle(CommandeValide, CancellationToken.None);

        _hasheur.Verify(h => h.Hacher(CommandeValide.MotDePasse), Times.Once);
    }

    [Fact]
    public async Task LaCreationVerifieUnicitéEmailAvantPersistance()
    {
        _repo.Setup(r => r.ExisteAvecEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(false);

        await _handler.Handle(CommandeValide, CancellationToken.None);

        _repo.Verify(r => r.ExisteAvecEmailAsync(
            It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
