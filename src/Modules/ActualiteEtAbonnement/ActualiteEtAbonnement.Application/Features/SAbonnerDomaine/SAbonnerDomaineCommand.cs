using MediatR;

namespace ActualiteEtAbonnement.Application.Features.SAbonnerDomaine;

public sealed class SAbonnerDomaineCommand : IRequest
{
    public Guid UtilisateurId { get; init; }
    public string DomaineMetier { get; init; } = string.Empty;
}
