using FluentAssertions;
using GestionIdentite.Application.Features.CreerCompteCandidat;
using GestionIdentite.Client.Controllers;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestionIdentite.Client.Tests.CreerCompteCandidat;

/// <summary>
/// Tests unitaires du Controller — Phase RED.
/// Le Controller ne contient aucune logique métier : on vérifie uniquement
/// le mapping HTTP ↔ Command et la translation des exceptions en codes HTTP.
///
/// Cf. .agent/skills/architecture-monolithe.md :
///   "Aucun traitement métier ne doit être implémenté dans les Controllers."
///   "Les Controllers délèguent systématiquement l'exécution à la couche
///    Application via MediatR."
///
/// CréerCompteCandidat est une action publique (inscription anonyme) :
/// aucun test de permission IVerificateurPermission n'est requis ici.
/// Cf. .agent/skills/regles-permissions.md — "fonctionnalité PROTÉGÉE".
/// </summary>
public sealed class ComptesCandidatsControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly ComptesCandidatsController _controller;

    private static readonly CreerCompteCandidatRequest RequeteValide =
        new("candidat@exemple.fr", "S3cr3t!Ok");

    public ComptesCandidatsControllerTests()
    {
        _controller = new ComptesCandidatsController(_sender.Object);
    }

    // -----------------------------------------------------------------------
    // Mapping HTTP — chemin nominal
    // -----------------------------------------------------------------------

    [Fact]
    public async Task LaCreationDUnCompteCandidatRetourneUnStatut201()
    {
        // Arrange
        _sender
            .Setup(s => s.Send(It.IsAny<CreerCompteCandidatCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreerCompteCandidatResponse(Guid.NewGuid()));

        // Act
        var result = await _controller.CreerCompteCandidat(RequeteValide, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task LaCreationDUnCompteCandidatRetourneLeCompteIdDansLaReponse()
    {
        // Arrange
        var compteId = Guid.NewGuid();
        _sender
            .Setup(s => s.Send(It.IsAny<CreerCompteCandidatCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreerCompteCandidatResponse(compteId));

        // Act
        var result = await _controller.CreerCompteCandidat(RequeteValide, CancellationToken.None)
            as CreatedAtActionResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeOfType<CreerCompteCandidatResponse>()
            .Which.CompteId.Should().Be(compteId);
    }

    [Fact]
    public async Task LaCreationDUnCompteCandidatMappeCorrectementLaRequeteVersLaCommand()
    {
        // Arrange
        CreerCompteCandidatCommand? commandCapturee = null;

        _sender
            .Setup(s => s.Send(It.IsAny<CreerCompteCandidatCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<CreerCompteCandidatResponse>, CancellationToken>(
                (cmd, _) => commandCapturee = cmd as CreerCompteCandidatCommand)
            .ReturnsAsync(new CreerCompteCandidatResponse(Guid.NewGuid()));

        var requete = new CreerCompteCandidatRequest("nouveau@exemple.fr", "MotD3Passe!");

        // Act
        await _controller.CreerCompteCandidat(requete, CancellationToken.None);

        // Assert
        commandCapturee.Should().NotBeNull();
        commandCapturee!.Email.Should().Be("nouveau@exemple.fr");
        commandCapturee.MotDePasse.Should().Be("MotD3Passe!");
    }

    [Fact]
    public async Task LaCreationDUnCompteCandidatAppelleSenderUneSeuleFois()
    {
        // Arrange
        _sender
            .Setup(s => s.Send(It.IsAny<CreerCompteCandidatCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreerCompteCandidatResponse(Guid.NewGuid()));

        // Act
        await _controller.CreerCompteCandidat(RequeteValide, CancellationToken.None);

        // Assert
        _sender.Verify(
            s => s.Send(It.IsAny<CreerCompteCandidatCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // -----------------------------------------------------------------------
    // Mapping HTTP — exceptions métier → codes d'erreur
    // -----------------------------------------------------------------------

    [Fact]
    public async Task LaCreationAvecUnEmailDejaUtiliseRetourneUnStatut409()
    {
        // Arrange
        _sender
            .Setup(s => s.Send(It.IsAny<CreerCompteCandidatCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EmailDejaUtiliseException("existant@exemple.fr"));

        var requeteDoublons = new CreerCompteCandidatRequest("existant@exemple.fr", "S3cr3t!Ok");

        // Act
        var result = await _controller.CreerCompteCandidat(requeteDoublons, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task LaReponseDeConflitContientLeMessageDErreur()
    {
        // Arrange
        var exception = new EmailDejaUtiliseException("existant@exemple.fr");

        _sender
            .Setup(s => s.Send(It.IsAny<CreerCompteCandidatCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.CreerCompteCandidat(
            new CreerCompteCandidatRequest("existant@exemple.fr", "S3cr3t!Ok"),
            CancellationToken.None) as ConflictObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new { message = exception.Message });
    }
}
