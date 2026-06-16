using FluentAssertions;
using GestionIdentite.Application.Features.BloquerCompte;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Moq;
using Xunit;

namespace GestionIdentite.Application.Tests.BloquerCompte;

/// <summary>
/// Fonctionnalité PROTÉGÉE : seul un Administrateur ayant Permission.BloquerCompte peut l'exécuter.
/// Cf. .agent/skills/regles-permissions.md
/// </summary>
public sealed class BloquerCompteHandlerTests
{
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly Mock<ICompteRepository> _repoCandidat = new();
    private readonly Mock<ICompteEntrepriseRepository> _repoEntreprise = new();
    private readonly BloquerCompteHandler _handler;

    private static readonly Guid AdminId = Guid.NewGuid();
    private static readonly Guid CandidatId = Guid.NewGuid();

    public BloquerCompteHandlerTests()
    {
        _handler = new BloquerCompteHandler(
            _verificateur.Object, _repoCandidat.Object, _repoEntreprise.Object);
    }

    private void AutoriserAdmin()
        => _verificateur
            .Setup(v => v.PossedePermissionAsync(AdminId, Permission.BloquerCompte, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

    private static CompteCandidat CandidatActif()
    {
        var email = Email.Creer("candidat@exemple.fr");
        return CompteCandidat.CréerAvecMotDePasseHache(email, "hash");
    }

    // -----------------------------------------------------------------------
    // Vérification des permissions — obligatoire (regles-permissions.md)
    // -----------------------------------------------------------------------

    [Fact]
    public async Task UnAdministrateurAutorisePeutBloquerUnCompteCandidat()
    {
        AutoriserAdmin();
        var candidat = CandidatActif();
        _repoCandidat.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(candidat);

        var command = new BloquerCompteCommand { AdministrateurId = AdminId, CompteId = CandidatId };

        await _handler.Handle(command, CancellationToken.None);

        candidat.EstBloque.Should().BeTrue();
    }

    [Fact]
    public async Task UnUtilisateurSansPermissionBloquerCompteLeveUnePermissionRefuseeException()
    {
        _verificateur
            .Setup(v => v.PossedePermissionAsync(AdminId, Permission.BloquerCompte, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new BloquerCompteCommand { AdministrateurId = AdminId, CompteId = CandidatId };

        var bloquer = () => _handler.Handle(command, CancellationToken.None);

        await bloquer.Should().ThrowAsync<PermissionRefuseeException>();
    }

    [Fact]
    public async Task LaPermissionEstVerifieeAvantToutAccesAuRepository()
    {
        _verificateur
            .Setup(v => v.PossedePermissionAsync(AdminId, Permission.BloquerCompte, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new BloquerCompteCommand { AdministrateurId = AdminId, CompteId = CandidatId };

        try { await _handler.Handle(command, CancellationToken.None); } catch { /* attendu */ }

        _repoCandidat.Verify(
            r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task BloquerUnCompteInexistantLeveUneCompteNonTrouveException()
    {
        AutoriserAdmin();
        _repoCandidat.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CompteCandidat?)null);
        _repoEntreprise.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((CompteEntreprise?)null);

        var command = new BloquerCompteCommand { AdministrateurId = AdminId, CompteId = Guid.NewGuid() };

        var bloquer = () => _handler.Handle(command, CancellationToken.None);

        await bloquer.Should().ThrowAsync<CompteNonTrouveException>();
    }

    [Fact]
    public async Task UnAdministrateurPeutBloquerUnCompteEntreprise()
    {
        AutoriserAdmin();
        var email = Email.Creer("entreprise@exemple.fr");
        var entreprise = CompteEntreprise.Creer(email, "TechCorp", "hash");

        _repoCandidat.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CompteCandidat?)null);
        _repoEntreprise.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(entreprise);

        var command = new BloquerCompteCommand { AdministrateurId = AdminId, CompteId = entreprise.Id };

        await _handler.Handle(command, CancellationToken.None);

        entreprise.EstBloque.Should().BeTrue();
    }
}
