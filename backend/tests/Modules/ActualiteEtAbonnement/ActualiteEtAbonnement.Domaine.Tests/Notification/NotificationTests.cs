using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Enums;
using FluentAssertions;
using Xunit;

namespace ActualiteEtAbonnement.Domaine.Tests.Notification;

public sealed class NotificationTests
{
    private static readonly Guid DestinatireId = Guid.NewGuid();

    [Fact]
    public void UneNotificationEstCreeeAvecLesInformationsValides()
    {
        var notification = Entites.Notification.Creer(
            DestinatireId, "Nouvelle annonce dans Cloud Azure",
            "Un poste de Dev .NET a été publié.", CanalDiffusion.Email);

        notification.Should().NotBeNull();
        notification.Id.Should().NotBe(Guid.Empty);
        notification.DestinataireId.Should().Be(DestinatireId);
        notification.Titre.Should().Be("Nouvelle annonce dans Cloud Azure");
        notification.Message.Should().Contain("Dev .NET");
        notification.Canal.Should().Be(CanalDiffusion.Email);
    }

    [Fact]
    public void UneNouvelleNotificationAUneDateDeCreationRenseignee()
    {
        var avant = DateTime.UtcNow;
        var notification = Entites.Notification.Creer(
            DestinatireId, "Titre", "Message.", CanalDiffusion.InApp);

        notification.DateCreation.Should().BeOnOrAfter(avant);
    }

    [Fact]
    public void UneNouvelleNotificationNEstPasLue()
    {
        var notification = Entites.Notification.Creer(
            DestinatireId, "Titre", "Message.", CanalDiffusion.Email);

        notification.EstLue.Should().BeFalse();
    }

    [Fact]
    public void CreerAvecDestinataireIdVideLeveArgumentException()
    {
        var creer = () => Entites.Notification.Creer(
            Guid.Empty, "Titre", "Message.", CanalDiffusion.Email);

        creer.Should().Throw<ArgumentException>();
    }
}
