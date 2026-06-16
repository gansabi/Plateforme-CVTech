using MediatR;

namespace GestionIdentite.Application.Features.CreerCompteAdministrateur;

public sealed class CreerCompteAdministrateurCommand : IRequest<CreerCompteAdministrateurResponse>
{
    public string Email { get; init; } = string.Empty;
    public string NomComplet { get; init; } = string.Empty;
    public string MotDePasse { get; init; } = string.Empty;
}
