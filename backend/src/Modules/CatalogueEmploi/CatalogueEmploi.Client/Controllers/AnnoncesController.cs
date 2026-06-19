using CatalogueEmploi.Application.Features.ConsulterAnnonces;
using CatalogueEmploi.Application.Features.PublierAnnonce;
using CatalogueEmploi.Domaine.Exceptions;
using FluentValidation;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueEmploi.Client.Controllers;

[ApiController]
[Route("api/catalogue/annonces")]
public sealed class AnnoncesController : ControllerBase
{
    private readonly ISender _sender;

    public AnnoncesController(ISender sender) => _sender = sender;

    /// <summary>GET api/catalogue/annonces — Consultation publique (visiteur anonyme autorisé)</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ConsulterAnnoncesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConsulterAnnonces(CancellationToken cancellationToken)
    {
        var reponse = await _sender.Send(new ConsulterAnnoncesQuery(), cancellationToken);
        return Ok(reponse);
    }

    /// <summary>POST api/catalogue/annonces — Publication par une entreprise</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PublierAnnonceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PublierAnnonce(
        [FromBody] PublierAnnonceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new PublierAnnonceCommand
        {
            UtilisateurId = request.UtilisateurId,
            Titre = request.Titre,
            Description = request.Description,
            TypeContrat = request.TypeContrat,
            DomaineMetier = request.DomaineMetier
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(ConsulterAnnonces), new { id = reponse.AnnonceId }, reponse);
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
