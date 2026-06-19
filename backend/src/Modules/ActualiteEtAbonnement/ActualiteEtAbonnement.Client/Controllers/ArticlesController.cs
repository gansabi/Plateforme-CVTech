using ActualiteEtAbonnement.Application.Features.PublierArticle;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActualiteEtAbonnement.Client.Controllers;

[ApiController]
[Route("api/actualite/articles")]
public sealed class ArticlesController : ControllerBase
{
    private readonly ISender _sender;

    public ArticlesController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType(typeof(PublierArticleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PublierArticle(
        [FromBody] PublierArticleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new PublierArticleCommand
        {
            AuteurId = request.AuteurId,
            Titre = request.Titre,
            Contenu = request.Contenu,
            DomaineMetier = request.DomaineMetier
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(PublierArticle), new { id = reponse.ArticleId }, reponse);
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }
}
