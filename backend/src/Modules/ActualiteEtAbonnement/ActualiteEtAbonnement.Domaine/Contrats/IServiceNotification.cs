using ActualiteEtAbonnement.Domaine.Entites;

namespace ActualiteEtAbonnement.Domaine.Contrats;

public interface IServiceNotification
{
    Task EnvoyerAsync(Notification notification, CancellationToken cancellationToken = default);
}
