using AppelOffreFreelance.Domaine.Contrats;
using MediatR;

namespace AppelOffreFreelance.Infrastructure.Evenements;

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

        await _mediator.Publish(new EvenementEnveloppe<T>(evenement), cancellationToken);
    }
}

public sealed class EvenementEnveloppe<T> : INotification where T : class
{
    public T Contenu { get; }
    public EvenementEnveloppe(T contenu) => Contenu = contenu;
}
