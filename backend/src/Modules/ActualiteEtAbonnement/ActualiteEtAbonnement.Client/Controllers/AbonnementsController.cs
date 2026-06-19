using ActualiteEtAbonnement.Application.Features.SAbonnerDomaine;
using ActualiteEtAbonnement.Domaine.Exceptions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActualiteEtAbonnement.Client.Controllers;

[ApiController]
[Route("api/actualite/abonnements")]
public sealed class AbonnementsController : ControllerBase
{
    private readonly ISender _sender;

    public AbonnementsController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SAbonner(
        [FromBody] SAbonnerDomaineRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SAbonnerDomaineCommand
        {
            UtilisateurId = request.UtilisateurId,
            DomaineMetier = request.DomaineMetier
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
        catch (AbonnementDejaExistantException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
