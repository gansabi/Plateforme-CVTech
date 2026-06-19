using ActualiteEtAbonnement.Application.Evenements;
using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Enums;
using CatalogueEmploi.Domaine.Evenements;
using MediatR;

namespace ActualiteEtAbonnement.Application.Features.NotifierAbonnes;

/// <summary>
/// Consomme l'événement AnnoncePubliee émis par CatalogueEmploi.
/// Génère une notification pour chaque abonné au domaine métier concerné.
/// Pas de vérification de permission : traitement interne déclenché par événement.
/// </summary>
public sealed class NotifierAbonnesAnnonceHandler
    : INotificationHandler<EvenementEnveloppe<AnnoncePubliee>>
{
    private readonly IAbonnementRepository _abonnementRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IServiceNotification _serviceNotification;

    public NotifierAbonnesAnnonceHandler(
        IAbonnementRepository abonnementRepository,
        INotificationRepository notificationRepository,
        IServiceNotification serviceNotification)
    {
        _abonnementRepository = abonnementRepository;
        _notificationRepository = notificationRepository;
        _serviceNotification = serviceNotification;
    }

    public async Task Handle(EvenementEnveloppe<AnnoncePubliee> enveloppe, CancellationToken cancellationToken)
    {
        var evenement = enveloppe.Contenu;
        var abonnes = await _abonnementRepository.ListerParDomaineAsync(evenement.DomaineMetier, cancellationToken);

        foreach (var abonne in abonnes)
        {
            var notification = Notification.Creer(
                abonne.UtilisateurId,
                $"Nouvelle annonce dans {evenement.DomaineMetier}",
                $"L'annonce \"{evenement.Titre}\" a été publiée.",
                CanalDiffusion.Email);

            await _notificationRepository.SauvegarderAsync(notification, cancellationToken);
            await _serviceNotification.EnvoyerAsync(notification, cancellationToken);
        }
    }
}
