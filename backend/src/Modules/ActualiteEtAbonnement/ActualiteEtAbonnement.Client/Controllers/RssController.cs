using ActualiteEtAbonnement.Application.Features.ConsulterFilRss;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;

namespace ActualiteEtAbonnement.Client.Controllers;

/// <summary>
/// Endpoint RSS 2.0 public et anonyme.
/// Ne contient que les articles éditoriaux (jamais d'annonces ni d'appels d'offre).
/// </summary>
[ApiController]
[Route("feed")]
public sealed class RssController : ControllerBase
{
    private readonly ISender _sender;

    public RssController(ISender sender) => _sender = sender;

    /// <summary>GET /feed/rss — Flux RSS 2.0 public</summary>
    [HttpGet("rss")]
    [Produces("application/rss+xml")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRss(
        [FromQuery] string? domaine,
        CancellationToken cancellationToken)
    {
        var query = new ConsulterFilRssQuery { DomaineMetier = domaine };
        var reponse = await _sender.Send(query, cancellationToken);

        var xml = GenererRss(reponse.Articles, domaine);
        return Content(xml, "application/rss+xml", Encoding.UTF8);
    }

    private static string GenererRss(IReadOnlyList<ArticleRssResume> articles, string? domaine)
    {
        var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("rss");
        writer.WriteAttributeString("version", "2.0");

        writer.WriteStartElement("channel");
        writer.WriteElementString("title", "Plateforme-CVTech — Actualité Tech");
        writer.WriteElementString("description",
            domaine != null
                ? $"Actualité tech — {domaine}"
                : "Fil d'actualité tech éditorial de Plateforme-CVTech");
        writer.WriteElementString("link", "https://plateforme-cvtech.local/feed/rss");
        writer.WriteElementString("language", "fr-FR");
        writer.WriteElementString("lastBuildDate", DateTime.UtcNow.ToString("R"));

        foreach (var article in articles)
        {
            writer.WriteStartElement("item");
            writer.WriteElementString("title", article.Titre);
            writer.WriteElementString("description", article.Contenu);
            writer.WriteElementString("category", article.DomaineMetier);
            writer.WriteElementString("pubDate", article.DatePublication.ToString("R"));
            writer.WriteElementString("guid", article.Id.ToString());
            writer.WriteEndElement();
        }

        writer.WriteEndElement(); // channel
        writer.WriteEndElement(); // rss
        writer.WriteEndDocument();
        writer.Flush();

        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
