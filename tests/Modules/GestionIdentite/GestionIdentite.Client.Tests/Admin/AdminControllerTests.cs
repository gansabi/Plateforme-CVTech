using FluentAssertions;
using GestionIdentite.Application.Features.BloquerCompte;
using GestionIdentite.Application.Features.ReactiverCompte;
using GestionIdentite.Client.Controllers;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestionIdentite.Client.Tests.Admin;

public sealed class AdminControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly AdminController _controller;

    private static readonly Guid AdminId = Guid.NewGuid();
    private static readonly Guid CompteId = Guid.NewGuid();

    public AdminControllerTests()
    {
        _controller = new AdminController(_sender.Object);
    }

    // -----------------------------------------------------------------------
    // BloquerCompte
    // -----------------------------------------------------------------------

    [Fact]
    public async Task BloquerUnCompteAvecPermissionRetourneUnStatut200()
    {
        _sender.Setup(s => s.Send(It.IsAny<BloquerCompteCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new BloquerCompteResponse(CompteId));

        var result = await _controller.BloquerCompte(
            new BloquerCompteRequest(AdminId, CompteId), CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task BloquerUnCompteSansPermissionRetourneUnStatut403()
    {
        _sender.Setup(s => s.Send(It.IsAny<BloquerCompteCommand>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new PermissionRefuseeException("Non autorisé."));

        var result = await _controller.BloquerCompte(
            new BloquerCompteRequest(AdminId, CompteId), CancellationToken.None);

        result.Should().BeOfType<ObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public async Task BloquerUnCompteInexistantRetourneUnStatut404()
    {
        _sender.Setup(s => s.Send(It.IsAny<BloquerCompteCommand>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new CompteNonTrouveException(CompteId));

        var result = await _controller.BloquerCompte(
            new BloquerCompteRequest(AdminId, CompteId), CancellationToken.None);

        result.Should().BeOfType<NotFoundObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    // -----------------------------------------------------------------------
    // ReactiverCompte
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ReactiverUnCompteAvecPermissionRetourneUnStatut200()
    {
        _sender.Setup(s => s.Send(It.IsAny<ReactiverCompteCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ReactiverCompteResponse(CompteId));

        var result = await _controller.ReactiverCompte(
            new ReactiverCompteRequest(AdminId, CompteId), CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task ReactiverUnCompteSansPermissionRetourneUnStatut403()
    {
        _sender.Setup(s => s.Send(It.IsAny<ReactiverCompteCommand>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new PermissionRefuseeException("Non autorisé."));

        var result = await _controller.ReactiverCompte(
            new ReactiverCompteRequest(AdminId, CompteId), CancellationToken.None);

        result.Should().BeOfType<ObjectResult>()
              .Which.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }
}
