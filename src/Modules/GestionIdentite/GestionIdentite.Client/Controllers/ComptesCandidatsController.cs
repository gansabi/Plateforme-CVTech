using FluentValidation;
using GestionIdentite.Application.Features.CreerCompteCandidat;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionIdentite.Client.Controllers;

/// <summary>
/// Porte d'entrée HTTP du module GestionIdentite pour les comptes candidats.
/// Responsabilité unique : mapping HTTP ↔ Command / Response.
/// Aucune logique métier ici.
/// Cf. .agent/skills/architecture-monolithe.md — section Couche Client.
/// </summary>
[ApiController]
[Route("api/identite/comptes-candidats")]
public sealed class ComptesCandidatsController : ControllerBase
{
    private readonly ISender _sender;

    public ComptesCandidatsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>POST api/identite/comptes-candidats</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreerCompteCandidatResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreerCompteCandidat(
        [FromBody] CreerCompteCandidatRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreerCompteCandidatCommand
        {
            Email = request.Email,
            MotDePasse = request.MotDePasse
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(CreerCompteCandidat),
                new { id = reponse.CompteId },
                reponse);
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
