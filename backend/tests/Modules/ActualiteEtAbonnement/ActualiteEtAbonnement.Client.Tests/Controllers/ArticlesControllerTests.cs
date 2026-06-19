using ActualiteEtAbonnement.Application.Features.PublierArticle;
using ActualiteEtAbonnement.Client.Controllers;
using FluentAssertions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ActualiteEtAbonnement.Client.Tests.Controllers;

public sealed class ArticlesControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly ArticlesController _controller;

    public ArticlesControllerTests()
    {
        _controller = new ArticlesController(_sender.Object);
    }

    [Fact]
    public async Task PublierArticleRetourne201QuandAutorise()
    {
        _sender.Setup(s => s.Send(It.IsAny<PublierArticleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PublierArticleResponse(Guid.NewGuid()));

        var request = new PublierArticleRequest(Guid.NewGuid(), "Titre", "Contenu.", "Cloud Azure");

        var result = await _controller.PublierArticle(request, CancellationToken.None);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task PublierArticleRetourne403QuandPermissionRefusee()
    {
        _sender.Setup(s => s.Send(It.IsAny<PublierArticleCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new PermissionRefuseeException("Refusé."));

        var request = new PublierArticleRequest(Guid.NewGuid(), "Titre", "Contenu.", "Cloud Azure");

        var result = await _controller.PublierArticle(request, CancellationToken.None);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(403);
    }
}
