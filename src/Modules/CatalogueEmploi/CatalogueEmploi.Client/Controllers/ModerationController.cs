using CatalogueEmploi.Application.Features.ModererAnnonce;
using CatalogueEmploi.Application.Features.SupprimerAnnonce;
using CatalogueEmploi.Domaine.Exceptions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueEmploi.Client.Controllers;

[ApiController]
[Route("api/catalogue/moderation")]
public sealed class ModerationController : ControllerBase
{
    private readonly ISender _sender;

    public ModerationController(ISender sender) => _sender = sender;

    /// <summary>POST api/catalogue/moderation/moderer — Modération par un administrateur</summary>
    [HttpPost("moderer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ModererAnnonce(
        [FromBody] ModererAnnonceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ModererAnnonceCommand
        {
            AdministrateurId = request.AdministrateurId,
            AnnonceId = request.AnnonceId
        };

        try
        {
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (AnnonceNonTrouveeException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>POST api/catalogue/moderation/supprimer — Suppression par un administrateur</summary>
    [HttpPost("supprimer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SupprimerAnnonce(
        [FromBody] SupprimerAnnonceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SupprimerAnnonceCommand
        {
            AdministrateurId = request.AdministrateurId,
            AnnonceId = request.AnnonceId
        };

        try
        {
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (AnnonceNonTrouveeException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
