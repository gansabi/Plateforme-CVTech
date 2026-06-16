using ActualiteEtAbonnement.Application.Evenements;
using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Enums;
using AppelOffreFreelance.Domaine.Evenements;
using MediatR;

namespace ActualiteEtAbonnement.Application.Features.NotifierAbonnes;

/// <summary>
/// Consomme l'événement AppelOffrePublie émis par AppelOffreFreelance.
/// Génère une notification pour chaque abonné au domaine métier concerné.
/// </summary>
public sealed class NotifierAbonnesAppelOffreHandler
    : INotificationHandler<EvenementEnveloppe<AppelOffrePublie>>
{
    private readonly IAbonnementRepository _abonnementRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IServiceNotification _serviceNotification;

    public NotifierAbonnesAppelOffreHandler(
        IAbonnementRepository abonnementRepository,
        INotificationRepository notificationRepository,
        IServiceNotification serviceNotification)
    {
        _abonnementRepository = abonnementRepository;
        _notificationRepository = notificationRepository;
        _serviceNotification = serviceNotification;
    }

    public async Task Handle(EvenementEnveloppe<AppelOffrePublie> enveloppe, CancellationToken cancellationToken)
    {
        var evenement = enveloppe.Contenu;
        var abonnes = await _abonnementRepository.ListerParDomaineAsync(evenement.DomaineMetier, cancellationToken);

        foreach (var abonne in abonnes)
        {
            var notification = Notification.Creer(
                abonne.UtilisateurId,
                $"Nouvel appel d'offre dans {evenement.DomaineMetier}",
                $"L'appel d'offre \"{evenement.Titre}\" a été publié.",
                CanalDiffusion.Email);

            await _notificationRepository.SauvegarderAsync(notification, cancellationToken);
            await _serviceNotification.EnvoyerAsync(notification, cancellationToken);
        }
    }
}
