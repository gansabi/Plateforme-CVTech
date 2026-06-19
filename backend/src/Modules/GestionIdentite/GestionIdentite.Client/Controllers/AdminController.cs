using GestionIdentite.Application.Features.BloquerCompte;
using GestionIdentite.Application.Features.ReactiverCompte;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionIdentite.Client.Controllers;

[ApiController]
[Route("api/identite/admin/comptes")]
public sealed class AdminController : ControllerBase
{
    private readonly ISender _sender;

    public AdminController(ISender sender) => _sender = sender;

    [HttpPost("{compteId:guid}/bloquer")]
    [ProducesResponseType(typeof(BloquerCompteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BloquerCompte(
        [FromBody] BloquerCompteRequest request,
        CancellationToken cancellationToken)
    {
        var command = new BloquerCompteCommand
        {
            AdministrateurId = request.AdministrateurId,
            CompteId = request.CompteId
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return Ok(reponse);
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (CompteNonTrouveException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{compteId:guid}/reactiver")]
    [ProducesResponseType(typeof(ReactiverCompteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReactiverCompte(
        [FromBody] ReactiverCompteRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReactiverCompteCommand
        {
            AdministrateurId = request.AdministrateurId,
            CompteId = request.CompteId
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return Ok(reponse);
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (CompteNonTrouveException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
