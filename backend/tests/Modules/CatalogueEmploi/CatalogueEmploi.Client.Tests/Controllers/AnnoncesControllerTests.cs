using CatalogueEmploi.Application.Features.ConsulterAnnonces;
using CatalogueEmploi.Application.Features.PublierAnnonce;
using CatalogueEmploi.Client.Controllers;
using FluentAssertions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CatalogueEmploi.Client.Tests.Controllers;

public sealed class AnnoncesControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly AnnoncesController _controller;

    public AnnoncesControllerTests()
    {
        _controller = new AnnoncesController(_sender.Object);
    }

    [Fact]
    public async Task ConsulterAnnoncesRetourneOkPourUnVisiteurAnonyme()
    {
        _sender.Setup(s => s.Send(It.IsAny<ConsulterAnnoncesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConsulterAnnoncesResponse([]));

        var result = await _controller.ConsulterAnnonces(CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PublierAnnonceRetourne201QuandAutorisee()
    {
        _sender.Setup(s => s.Send(It.IsAny<PublierAnnonceCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PublierAnnonceResponse(Guid.NewGuid()));

        var request = new PublierAnnonceRequest(Guid.NewGuid(), "Dev .NET", "Mission.", "CDI", "Cloud Azure");
        var result = await _controller.PublierAnnonce(request, CancellationToken.None);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task PublierAnnonceRetourne403QuandPermissionRefusee()
    {
        _sender.Setup(s => s.Send(It.IsAny<PublierAnnonceCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new PermissionRefuseeException("Refusé."));

        var request = new PublierAnnonceRequest(Guid.NewGuid(), "Dev", "Desc.", "CDI", "Cloud");
        var result = await _controller.PublierAnnonce(request, CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(403);
    }
}
