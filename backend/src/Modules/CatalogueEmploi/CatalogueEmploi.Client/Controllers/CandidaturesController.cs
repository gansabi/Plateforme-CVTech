using CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;
using CatalogueEmploi.Application.Features.PostulerAnnonce;
using CatalogueEmploi.Domaine.Exceptions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueEmploi.Client.Controllers;

[ApiController]
[Route("api/catalogue/candidatures")]
public sealed class CandidaturesController : ControllerBase
{
    private readonly ISender _sender;

    public CandidaturesController(ISender sender) => _sender = sender;

    /// <summary>POST api/catalogue/candidatures — Postuler à une annonce</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Postuler(
        [FromBody] PostulerAnnonceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new PostulerAnnonceCommand
        {
            CandidatId = request.CandidatId,
            AnnonceId = request.AnnonceId,
            LettreMotivation = request.LettreMotivation
        };

        try
        {
            await _sender.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (AnnonceNonTrouveeException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (CandidatureDejaExistanteException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>GET api/catalogue/candidatures?utilisateurId=...&annonceId=... — Consulter candidatures reçues</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ConsulterCandidaturesRecuesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConsulterCandidaturesRecues(
        [FromQuery] Guid utilisateurId,
        [FromQuery] Guid annonceId,
        CancellationToken cancellationToken)
    {
        var query = new ConsulterCandidaturesRecuesQuery
        {
            UtilisateurId = utilisateurId,
            AnnonceId = annonceId
        };

        try
        {
            var reponse = await _sender.Send(query, cancellationToken);
            return Ok(reponse);
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }
}
