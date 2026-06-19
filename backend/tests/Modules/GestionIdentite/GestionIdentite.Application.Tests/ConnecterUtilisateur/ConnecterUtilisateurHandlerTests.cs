using FluentAssertions;
using GestionIdentite.Application.Features.ConnecterUtilisateur;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Moq;
using Xunit;

namespace GestionIdentite.Application.Tests.ConnecterUtilisateur;

public sealed class ConnecterUtilisateurHandlerTests
{
    private readonly Mock<IRegistreUtilisateurs> _registre = new();
    private readonly Mock<IHasheurMotDePasse> _hasheur = new();
    private readonly ConnecterUtilisateurHandler _handler;

    private static readonly ConnecterUtilisateurCommand CommandeValide = new()
    {
        Email = "candidat@exemple.fr",
        MotDePasse = "S3cr3t!Ok"
    };

    private static readonly InfosUtilisateur CandidatActif = new(
        Guid.NewGuid(), "candidat@exemple.fr", Role.Candidat, "hash_valide", EstBloque: false);

    public ConnecterUtilisateurHandlerTests()
    {
        _handler = new ConnecterUtilisateurHandler(_registre.Object, _hasheur.Object);
    }

    [Fact]
    public async Task LaConnexionAvecIdentifiantsValidesRetourneUnUtilisateurConnecte()
    {
        _registre.Setup(r => r.TrouverParEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(CandidatActif);
        _hasheur.Setup(h => h.Verifier(CommandeValide.MotDePasse, CandidatActif.MotDePasseHache))
                .Returns(true);

        var reponse = await _handler.Handle(CommandeValide, CancellationToken.None);

        reponse.UtilisateurId.Should().Be(CandidatActif.Id);
        reponse.Role.Should().Be(Role.Candidat);
    }

    [Fact]
    public async Task LaConnexionAvecEmailInexistantLeveUneCredentielsInvalidesException()
    {
        _registre.Setup(r => r.TrouverParEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((InfosUtilisateur?)null);

        var connecter = () => _handler.Handle(CommandeValide, CancellationToken.None);

        await connecter.Should().ThrowAsync<CredentielsInvalidesException>();
    }

    [Fact]
    public async Task LaConnexionAvecMotDePasseIncorrectLeveUneCredentielsInvalidesException()
    {
        _registre.Setup(r => r.TrouverParEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(CandidatActif);
        _hasheur.Setup(h => h.Verifier(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var connecter = () => _handler.Handle(CommandeValide, CancellationToken.None);

        await connecter.Should().ThrowAsync<CredentielsInvalidesException>();
    }

    [Fact]
    public async Task LaConnexionDUnCompteBloqueLeveUneCompteEstBloqueException()
    {
        var compteBloque = CandidatActif with { EstBloque = true };

        _registre.Setup(r => r.TrouverParEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(compteBloque);
        _hasheur.Setup(h => h.Verifier(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var connecter = () => _handler.Handle(CommandeValide, CancellationToken.None);

        await connecter.Should().ThrowAsync<CompteEstBloqueException>();
    }

    [Fact]
    public async Task LaConnexionRetourneLEmailEtLeRoleDeLutilisateur()
    {
        _registre.Setup(r => r.TrouverParEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(CandidatActif);
        _hasheur.Setup(h => h.Verifier(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var reponse = await _handler.Handle(CommandeValide, CancellationToken.None);

        reponse.Email.Should().Be(CandidatActif.Email);
        reponse.Role.Should().Be(CandidatActif.Role);
    }
}
