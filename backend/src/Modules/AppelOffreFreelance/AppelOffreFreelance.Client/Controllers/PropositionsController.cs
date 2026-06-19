using AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;
using AppelOffreFreelance.Application.Features.SoumettreProposition;
using AppelOffreFreelance.Domaine.Exceptions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppelOffreFreelance.Client.Controllers;

[ApiController]
[Route("api/freelance/propositions")]
public sealed class PropositionsController : ControllerBase
{
    private readonly ISender _sender;

    public PropositionsController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SoumettreProposition(
        [FromBody] SoumettrePropositionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SoumettrePropositionCommand
        {
            CandidatId = request.CandidatId,
            AppelOffreId = request.AppelOffreId,
            TarifJournalier = request.TarifJournalier,
            DureeJours = request.DureeJours,
            Methodologie = request.Methodologie
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
        catch (AppelOffreNonTrouveException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (PropositionDejaExistanteException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ConsulterPropositionsRecuesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConsulterPropositionsRecues(
        [FromQuery] Guid utilisateurId,
        [FromQuery] Guid appelOffreId,
        CancellationToken cancellationToken)
    {
        var query = new ConsulterPropositionsRecuesQuery
        {
            UtilisateurId = utilisateurId,
            AppelOffreId = appelOffreId
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
