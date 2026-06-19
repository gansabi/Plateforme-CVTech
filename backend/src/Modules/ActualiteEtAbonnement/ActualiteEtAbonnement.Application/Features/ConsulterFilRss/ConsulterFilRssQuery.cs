using MediatR;

namespace ActualiteEtAbonnement.Application.Features.ConsulterFilRss;

public sealed class ConsulterFilRssQuery : IRequest<ConsulterFilRssResponse>
{
    public string? DomaineMetier { get; init; }
}
