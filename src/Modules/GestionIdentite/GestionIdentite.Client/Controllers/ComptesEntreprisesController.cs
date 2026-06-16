using FluentValidation;
using GestionIdentite.Application.Features.CreerCompteEntreprise;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionIdentite.Client.Controllers;

[ApiController]
[Route("api/identite/comptes-entreprises")]
public sealed class ComptesEntreprisesController : ControllerBase
{
    private readonly ISender _sender;

    public ComptesEntreprisesController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType(typeof(CreerCompteEntrepriseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreerCompteEntreprise(
        [FromBody] CreerCompteEntrepriseRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreerCompteEntrepriseCommand
        {
            Email = request.Email,
            MotDePasse = request.MotDePasse,
            NomEntreprise = request.NomEntreprise
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreerCompteEntreprise), new { id = reponse.CompteId }, reponse);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { erreurs = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (EmailDejaUtiliseException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
