using AppelOffreFreelance.Application.Features.ConsulterAppelsOffre;
using AppelOffreFreelance.Application.Features.PublierAppelOffre;
using FluentValidation;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppelOffreFreelance.Client.Controllers;

[ApiController]
[Route("api/freelance/appels-offre")]
public sealed class AppelsOffreController : ControllerBase
{
    private readonly ISender _sender;

    public AppelsOffreController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(ConsulterAppelsOffreResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConsulterAppelsOffre(CancellationToken cancellationToken)
    {
        var reponse = await _sender.Send(new ConsulterAppelsOffreQuery(), cancellationToken);
        return Ok(reponse);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PublierAppelOffreResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PublierAppelOffre(
        [FromBody] PublierAppelOffreRequest request,
        CancellationToken cancellationToken)
    {
        var command = new PublierAppelOffreCommand
        {
            UtilisateurId = request.UtilisateurId,
            Titre = request.Titre,
            Description = request.Description,
            DomaineMetier = request.DomaineMetier,
            TjmMinimum = request.TjmMinimum,
            TjmMaximum = request.TjmMaximum,
            DateLimite = request.DateLimite
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(ConsulterAppelsOffre), new { id = reponse.AppelOffreId }, reponse);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { erreurs = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }
}
