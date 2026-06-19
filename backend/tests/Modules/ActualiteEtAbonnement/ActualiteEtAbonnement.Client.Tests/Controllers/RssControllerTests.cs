using ActualiteEtAbonnement.Application.Features.ConsulterFilRss;
using ActualiteEtAbonnement.Client.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ActualiteEtAbonnement.Client.Tests.Controllers;

public sealed class RssControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly RssController _controller;

    public RssControllerTests()
    {
        _controller = new RssController(_sender.Object);
    }

    [Fact]
    public async Task GetRssRetourneUnContenuRssXml()
    {
        _sender.Setup(s => s.Send(It.IsAny<ConsulterFilRssQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConsulterFilRssResponse([]));

        var result = await _controller.GetRss(null, CancellationToken.None);

        var contentResult = result.Should().BeOfType<ContentResult>().Subject;
        contentResult.ContentType.Should().Contain("application/rss+xml");
        contentResult.Content.Should().Contain("<rss");
        contentResult.Content.Should().Contain("version=\"2.0\"");
    }

    [Fact]
    public async Task LeFluxRssNeContientQueDesArticlesEditoriaux()
    {
        var articles = new List<ArticleRssResume>
        {
            new(Guid.NewGuid(), "Tendances Cloud", "Contenu éditorial.", "Cloud Azure", DateTime.UtcNow)
        };
        _sender.Setup(s => s.Send(It.IsAny<ConsulterFilRssQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConsulterFilRssResponse(articles.AsReadOnly()));

        var result = await _controller.GetRss(null, CancellationToken.None);

        var contentResult = result.Should().BeOfType<ContentResult>().Subject;
        contentResult.Content.Should().Contain("Tendances Cloud");
        contentResult.Content.Should().NotContain("annonce");
        contentResult.Content.Should().NotContain("appel d'offre");
    }
}
