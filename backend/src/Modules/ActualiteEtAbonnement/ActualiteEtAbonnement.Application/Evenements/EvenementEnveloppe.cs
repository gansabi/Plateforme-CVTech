using MediatR;

namespace ActualiteEtAbonnement.Application.Evenements;

/// <summary>
/// Enveloppe générique pour les événements métier inter-modules.
/// Permet de consommer des événements via MediatR INotification.
/// </summary>
public sealed class EvenementEnveloppe<T> : INotification where T : class
{
    public T Contenu { get; }
    public EvenementEnveloppe(T contenu) => Contenu = contenu;
}
