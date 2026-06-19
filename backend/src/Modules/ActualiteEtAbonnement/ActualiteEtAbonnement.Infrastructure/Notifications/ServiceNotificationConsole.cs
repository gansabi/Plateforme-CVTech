using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;

namespace ActualiteEtAbonnement.Infrastructure.Notifications;

/// <summary>
/// Implémentation de démonstration du service de notification.
/// Écrit les notifications dans la console (simule un envoi e-mail).
/// </summary>
public sealed class ServiceNotificationConsole : IServiceNotification
{
    public Task EnvoyerAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[NOTIFICATION] → {notification.DestinataireId} | {notification.Canal} | {notification.Titre} : {notification.Message}");
        return Task.CompletedTask;
    }
}
