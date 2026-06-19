using MediatR;

namespace AppelOffreFreelance.Application.Features.SupprimerAppelOffre;

public sealed class SupprimerAppelOffreCommand : IRequest
{
    public Guid AdministrateurId { get; init; }
    public Guid AppelOffreId { get; init; }
}
