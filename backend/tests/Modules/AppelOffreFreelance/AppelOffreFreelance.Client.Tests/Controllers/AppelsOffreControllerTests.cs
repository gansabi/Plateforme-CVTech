using AppelOffreFreelance.Application.Features.ConsulterAppelsOffre;
using AppelOffreFreelance.Application.Features.PublierAppelOffre;
using AppelOffreFreelance.Client.Controllers;
using FluentAssertions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AppelOffreFreelance.Client.Tests.Controllers;

public sealed class AppelsOffreControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly AppelsOffreController _controller;

    public AppelsOffreControllerTests()
    {
        _controller = new AppelsOffreController(_sender.Object);
    }

    [Fact]
    public async Task ConsulterAppelsOffreRetourneOk()
    {
        _sender.Setup(s => s.Send(It.IsAny<ConsulterAppelsOffreQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConsulterAppelsOffreResponse([]));

        var result = await _controller.ConsulterAppelsOffre(CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PublierAppelOffreRetourne201QuandAutorise()
    {
        _sender.Setup(s => s.Send(It.IsAny<PublierAppelOffreCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PublierAppelOffreResponse(Guid.NewGuid()));

        var request = new PublierAppelOffreRequest(
            Guid.NewGuid(), "Mission", "Desc.", "Cloud", 500, 800, DateTime.UtcNow.AddDays(30));

        var result = await _controller.PublierAppelOffre(request, CancellationToken.None);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task PublierAppelOffreRetourne403QuandPermissionRefusee()
    {
        _sender.Setup(s => s.Send(It.IsAny<PublierAppelOffreCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new PermissionRefuseeException("Refusé."));

        var request = new PublierAppelOffreRequest(
            Guid.NewGuid(), "Mission", "Desc.", "Cloud", 500, 800, DateTime.UtcNow.AddDays(30));

        var result = await _controller.PublierAppelOffre(request, CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(403);
    }
}
