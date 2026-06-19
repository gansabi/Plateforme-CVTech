using AppelOffreFreelance.Application.Features.ModererAppelOffre;
using AppelOffreFreelance.Application.Features.SupprimerAppelOffre;
using AppelOffreFreelance.Domaine.Exceptions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppelOffreFreelance.Client.Controllers;

[ApiController]
[Route("api/freelance/moderation")]
public sealed class ModerationAppelOffreController : ControllerBase
{
    private readonly ISender _sender;

    public ModerationAppelOffreController(ISender sender) => _sender = sender;

    [HttpPost("moderer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ModererAppelOffre(
        [FromBody] ModererAppelOffreRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(new ModererAppelOffreCommand
            {
                AdministrateurId = request.AdministrateurId,
                AppelOffreId = request.AppelOffreId
            }, cancellationToken);
            return NoContent();
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (AppelOffreNonTrouveException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("supprimer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SupprimerAppelOffre(
        [FromBody] SupprimerAppelOffreRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(new SupprimerAppelOffreCommand
            {
                AdministrateurId = request.AdministrateurId,
                AppelOffreId = request.AppelOffreId
            }, cancellationToken);
            return NoContent();
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (AppelOffreNonTrouveException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
