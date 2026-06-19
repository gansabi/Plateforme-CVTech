using MediatR;

namespace GestionIdentite.Application.Features.CreerCompteEntreprise;

public sealed record CreerCompteEntrepriseCommand : IRequest<CreerCompteEntrepriseResponse>
{
    public required string Email { get; init; }
    public required string MotDePasse { get; init; }
    public required string NomEntreprise { get; init; }
}
