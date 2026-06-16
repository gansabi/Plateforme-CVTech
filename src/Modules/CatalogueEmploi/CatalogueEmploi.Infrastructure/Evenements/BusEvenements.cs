using CatalogueEmploi.Domaine.Contrats;
using MediatR;

namespace CatalogueEmploi.Infrastructure.Evenements;

/// <summary>
/// Implémentation du bus d'événements interne via MediatR.
/// Les événements sont publiés en mémoire pour être consommés par les autres modules.
/// </summary>
public sealed class BusEvenements : IBusEvenements
{
    private readonly IMediator _mediator;

    public BusEvenements(IMediator mediator)
        => _mediator = mediator;

    public async Task PublierAsync<T>(T evenement, CancellationToken cancellationToken = default) where T : class
    {
        if (evenement is INotification notification)
        {
            await _mediator.Publish(notification, cancellationToken);
            return;
        }

        // Pour les événements qui ne sont pas des INotification MediatR,
        // on les wrappe dans une notification générique.
        await _mediator.Publish(new EvenementEnveloppe<T>(evenement), cancellationToken);
    }
}

/// <summary>
/// Enveloppe générique permettant de publier tout événement métier via MediatR.
/// </summary>
public sealed class EvenementEnveloppe<T> : INotification where T : class
{
    public T Contenu { get; }

    public EvenementEnveloppe(T contenu) => Contenu = contenu;
}
