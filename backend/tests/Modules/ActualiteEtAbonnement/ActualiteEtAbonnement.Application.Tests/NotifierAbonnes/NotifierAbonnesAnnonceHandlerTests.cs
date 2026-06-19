using ActualiteEtAbonnement.Application.Evenements;
using ActualiteEtAbonnement.Application.Features.NotifierAbonnes;
using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Enums;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using CatalogueEmploi.Domaine.Evenements;
using FluentAssertions;
using Moq;
using Xunit;

namespace ActualiteEtAbonnement.Application.Tests.NotifierAbonnes;

public sealed class NotifierAbonnesAnnonceHandlerTests
{
    private readonly Mock<IAbonnementRepository> _abonnementRepo = new();
    private readonly Mock<INotificationRepository> _notificationRepo = new();
    private readonly Mock<IServiceNotification> _serviceNotification = new();
    private readonly NotifierAbonnesAnnonceHandler _handler;

    public NotifierAbonnesAnnonceHandlerTests()
    {
        _handler = new NotifierAbonnesAnnonceHandler(
            _abonnementRepo.Object, _notificationRepo.Object, _serviceNotification.Object);
    }

    [Fact]
    public async Task UneAnnoncePublieeDeclencheUneNotificationPourLesAbonnesDuDomaine()
    {
        var abonne = Abonnement.Creer(Guid.NewGuid(), DomaineMetier.Creer("Cloud Azure"));
        _abonnementRepo.Setup(r => r.ListerParDomaineAsync("Cloud Azure", It.IsAny<CancellationToken>()))
            .ReturnsAsync([abonne]);

        var evenement = new EvenementEnveloppe<AnnoncePubliee>(
            new AnnoncePubliee(Guid.NewGuid(), Guid.NewGuid(), "Dev .NET", "Cloud Azure", DateTime.UtcNow));

        await _handler.Handle(evenement, CancellationToken.None);

        _notificationRepo.Verify(r => r.SauvegarderAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Once);
        _serviceNotification.Verify(s => s.EnvoyerAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AucuneNotificationSiAucunAbonneDansLeDomaine()
    {
        _abonnementRepo.Setup(r => r.ListerParDomaineAsync("Data Science", It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var evenement = new EvenementEnveloppe<AnnoncePubliee>(
            new AnnoncePubliee(Guid.NewGuid(), Guid.NewGuid(), "Data Analyst", "Data Science", DateTime.UtcNow));

        await _handler.Handle(evenement, CancellationToken.None);

        _notificationRepo.Verify(r => r.SauvegarderAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
