using MediatR;

namespace GestionIdentite.Application.Features.ConnecterUtilisateur;

public sealed record ConnecterUtilisateurCommand : IRequest<ConnecterUtilisateurResponse>
{
    public required string Email { get; init; }
    public required string MotDePasse { get; init; }
}
