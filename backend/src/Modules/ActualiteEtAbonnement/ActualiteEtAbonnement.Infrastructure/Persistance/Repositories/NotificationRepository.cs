using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;

namespace ActualiteEtAbonnement.Infrastructure.Persistance.Repositories;

public sealed class NotificationRepository : INotificationRepository
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;

    public NotificationRepository(ActualiteEtAbonnementDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _dbContext.Notifications.AddAsync(notification, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
