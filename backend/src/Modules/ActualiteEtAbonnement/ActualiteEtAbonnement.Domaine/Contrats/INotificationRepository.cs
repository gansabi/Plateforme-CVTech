using ActualiteEtAbonnement.Domaine.Entites;

namespace ActualiteEtAbonnement.Domaine.Contrats;

public interface INotificationRepository
{
    Task SauvegarderAsync(Notification notification, CancellationToken cancellationToken = default);
}
