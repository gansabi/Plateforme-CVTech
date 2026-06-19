using FluentValidation;
using GestionIdentite.Application.Features.CreerCompteAdministrateur;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionIdentite.Client.Controllers;

[ApiController]
[Route("api/identite/administrateurs")]
public sealed class GestionAdministrateursController : ControllerBase
{
    private readonly ISender _sender;

    public GestionAdministrateursController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType(typeof(CreerCompteAdministrateurResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreerCompteAdministrateur(
        [FromBody] CreerCompteAdministrateurRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreerCompteAdministrateurCommand
        {
            Email = request.Email,
            NomComplet = request.NomComplet,
            MotDePasse = request.MotDePasse
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreerCompteAdministrateur), new { id = reponse.CompteId }, reponse);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (EmailDejaUtiliseException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
