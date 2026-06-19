using MediatR;

namespace GestionIdentite.Application.Features.CreerCompteCandidat;

public sealed record CreerCompteCandidatCommand : IRequest<CreerCompteCandidatResponse>
{
    public required string Email { get; init; }
    public required string MotDePasse { get; init; }
}
