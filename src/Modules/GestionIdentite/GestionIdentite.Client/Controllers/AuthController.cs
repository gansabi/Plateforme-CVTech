using GestionIdentite.Application.Features.ConnecterUtilisateur;
using GestionIdentite.Domaine.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionIdentite.Client.Controllers;

[ApiController]
[Route("api/identite/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender) => _sender = sender;

    [HttpPost("connexion")]
    [ProducesResponseType(typeof(ConnecterUtilisateurResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConnecterUtilisateur(
        [FromBody] ConnecterUtilisateurRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ConnecterUtilisateurCommand
        {
            Email = request.Email,
            MotDePasse = request.MotDePasse
        };

        try
        {
            var reponse = await _sender.Send(command, cancellationToken);
            return Ok(new
            {
                utilisateurId = reponse.UtilisateurId,
                email = reponse.Email,
                role = reponse.Role.ToString()
            });
        }
        catch (CompteEstBloqueException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (CredentielsInvalidesException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
