using MediatR;

namespace AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;

public sealed class ConsulterPropositionsRecuesQuery : IRequest<ConsulterPropositionsRecuesResponse>
{
    public Guid UtilisateurId { get; init; }
    public Guid AppelOffreId { get; init; }
}
