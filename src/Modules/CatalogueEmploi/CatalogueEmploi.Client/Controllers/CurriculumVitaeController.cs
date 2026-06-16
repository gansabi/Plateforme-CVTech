using CatalogueEmploi.Application.Features.CreerCurriculumVitae;
using CatalogueEmploi.Application.Features.ModifierCurriculumVitae;
using CatalogueEmploi.Domaine.Exceptions;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueEmploi.Client.Controllers;

[ApiController]
[Route("api/catalogue/cv")]
public sealed class CurriculumVitaeController : ControllerBase
{
    private readonly ISender _sender;

    public CurriculumVitaeController(ISender sender) => _sender = sender;

    /// <summary>POST api/catalogue/cv — Créer un CV</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreerCurriculumVitaeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreerCurriculumVitae(
        [FromBody] CreerCurriculumVitaeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreerCurriculumVitaeCommand
        {
            CandidatId = request.CandidatId,
            Titre = request.Titre,
            Resume = request.Resume,
            CompetencesPrincipales = request.CompetencesPrincipales
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreerCurriculumVitae), new { id = reponse.CurriculumVitaeId }, reponse);
        }
        catch (PermissionRefuseeException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    /// <summary>PUT api/catalogue/cv — Modifier un CV</summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ModifierCurriculumVitae(
        [FromBody] ModifierCurriculumVitaeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ModifierCurriculumVitaeCommand
        {
            CandidatId = request.CandidatId,
            Titre = request.Titre,
            Resume = request.Resume,
            CompetencesPrincipales = request.CompetencesPrincipales
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
        catch (CurriculumVitaeNonTrouveException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
