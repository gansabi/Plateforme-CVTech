using FluentAssertions;
using GestionIdentite.Application.Features.CreerCompteAdministrateur;
using GestionIdentite.Client.Controllers;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestionIdentite.Client.Tests.CreerCompteAdministrateur;

public sealed class GestionAdministrateursControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly GestionAdministrateursController _controller;

    private static readonly CreerCompteAdministrateurRequest RequeteValide =
        new("admin@exemple.fr", "Alice Martin", "S3cur3Admin!");

    public GestionAdministrateursControllerTests()
    {
        _controller = new GestionAdministrateursController(_sender.Object);
    }

    [Fact]
    public async Task LaCreationDUnCompteAdminRetourneUnStatut201()
    {
        _sender.Setup(s => s.Send(It.IsAny<CreerCompteAdministrateurCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new CreerCompteAdministrateurResponse(Guid.NewGuid()));

        var result = await _controller.CreerCompteAdministrateur(RequeteValide, CancellationToken.None);

        result.Should().BeOfType<CreatedAtActionResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task LaCreationAvecEmailDejaUtiliseRetourneUnStatut409()
    {
        _sender.Setup(s => s.Send(It.IsAny<CreerCompteAdministrateurCommand>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new EmailDejaUtiliseException("admin@exemple.fr"));

        var result = await _controller.CreerCompteAdministrateur(RequeteValide, CancellationToken.None);

        result.Should().BeOfType<ConflictObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [Fact]
    public async Task LaCreationMappeCorrectementLaRequeteVersLaCommand()
    {
        CreerCompteAdministrateurCommand? commandeCapturee = null;
        _sender.Setup(s => s.Send(It.IsAny<CreerCompteAdministrateurCommand>(), It.IsAny<CancellationToken>()))
               .Callback<IRequest<CreerCompteAdministrateurResponse>, CancellationToken>(
                   (cmd, _) => commandeCapturee = cmd as CreerCompteAdministrateurCommand)
               .ReturnsAsync(new CreerCompteAdministrateurResponse(Guid.NewGuid()));

        await _controller.CreerCompteAdministrateur(RequeteValide, CancellationToken.None);

        commandeCapturee!.Email.Should().Be(RequeteValide.Email);
        commandeCapturee.NomComplet.Should().Be(RequeteValide.NomComplet);
    }
}
