using MediatR;

namespace GestionIdentite.Application.Features.ReactiverCompte;

public sealed record ReactiverCompteCommand : IRequest<ReactiverCompteResponse>
{
    public required Guid AdministrateurId { get; init; }
    public required Guid CompteId { get; init; }
}
