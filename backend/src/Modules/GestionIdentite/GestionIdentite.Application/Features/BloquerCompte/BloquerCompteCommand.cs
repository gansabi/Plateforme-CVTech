using MediatR;

namespace GestionIdentite.Application.Features.BloquerCompte;

public sealed record BloquerCompteCommand : IRequest<BloquerCompteResponse>
{
    public required Guid AdministrateurId { get; init; }
    public required Guid CompteId { get; init; }
}
