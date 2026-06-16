using FluentAssertions;
using GestionIdentite.Application.Features.ReactiverCompte;
using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;
using Moq;
using Xunit;

namespace GestionIdentite.Application.Tests.ReactiverCompte;

/// <summary>
/// Fonctionnalité PROTÉGÉE : seul un Administrateur ayant Permission.ReactiverCompte peut l'exécuter.
/// </summary>
public sealed class ReactiverCompteHandlerTests
{
    private readonly Mock<IVerificateurPermission> _verificateur = new();
    private readonly Mock<ICompteRepository> _repoCandidat = new();
    private readonly Mock<ICompteEntrepriseRepository> _repoEntreprise = new();
    private readonly ReactiverCompteHandler _handler;

    private static readonly Guid AdminId = Guid.NewGuid();
    private static readonly Guid CandidatId = Guid.NewGuid();

    public ReactiverCompteHandlerTests()
    {
        _handler = new ReactiverCompteHandler(
            _verificateur.Object, _repoCandidat.Object, _repoEntreprise.Object);
    }

    private void AutoriserAdmin()
        => _verificateur
            .Setup(v => v.PossedePermissionAsync(AdminId, Permission.ReactiverCompte, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

    private static CompteCandidat CandidatBloque()
    {
        var email = Email.Creer("candidat@exemple.fr");
        var c = CompteCandidat.CréerAvecMotDePasseHache(email, "hash");
        c.Bloquer();
        return c;
    }

    [Fact]
    public async Task UnAdministrateurAutorisePeutReactiverUnCompteCandidat()
    {
        AutoriserAdmin();
        var candidat = CandidatBloque();
        _repoCandidat.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(candidat);

        var command = new ReactiverCompteCommand { AdministrateurId = AdminId, CompteId = CandidatId };

        await _handler.Handle(command, CancellationToken.None);

        candidat.EstBloque.Should().BeFalse();
    }

    [Fact]
    public async Task UnUtilisateurSansPermissionReactiverCompteLeveUnePermissionRefuseeException()
    {
        _verificateur
            .Setup(v => v.PossedePermissionAsync(AdminId, Permission.ReactiverCompte, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new ReactiverCompteCommand { AdministrateurId = AdminId, CompteId = CandidatId };

        var reactiver = () => _handler.Handle(command, CancellationToken.None);

        await reactiver.Should().ThrowAsync<PermissionRefuseeException>();
    }

    [Fact]
    public async Task LaPermissionEstVerifieeAvantToutAccesAuRepository()
    {
        _verificateur
            .Setup(v => v.PossedePermissionAsync(AdminId, Permission.ReactiverCompte, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new ReactiverCompteCommand { AdministrateurId = AdminId, CompteId = CandidatId };

        try { await _handler.Handle(command, CancellationToken.None); } catch { /* attendu */ }

        _repoCandidat.Verify(
            r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ReactiverUnCompteInexistantLeveUneCompteNonTrouveException()
    {
        AutoriserAdmin();
        _repoCandidat.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CompteCandidat?)null);
        _repoEntreprise.Setup(r => r.TrouverParIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((CompteEntreprise?)null);

        var command = new ReactiverCompteCommand { AdministrateurId = AdminId, CompteId = Guid.NewGuid() };

        var reactiver = () => _handler.Handle(command, CancellationToken.None);

        await reactiver.Should().ThrowAsync<CompteNonTrouveException>();
    }
}
