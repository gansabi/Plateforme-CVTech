using MediatR;

namespace AppelOffreFreelance.Application.Features.ModererAppelOffre;

public sealed class ModererAppelOffreCommand : IRequest
{
    public Guid AdministrateurId { get; init; }
    public Guid AppelOffreId { get; init; }
}
