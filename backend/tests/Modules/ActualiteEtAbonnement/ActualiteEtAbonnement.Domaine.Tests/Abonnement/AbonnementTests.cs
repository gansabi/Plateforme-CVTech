using ActualiteEtAbonnement.Domaine.Entites;
using ActualiteEtAbonnement.Domaine.Exceptions;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;
using FluentAssertions;
using Xunit;

namespace ActualiteEtAbonnement.Domaine.Tests.Abonnement;

public sealed class AbonnementTests
{
    private static readonly Guid UtilisateurId = Guid.NewGuid();
    private static DomaineMetier DomaineValide() => DomaineMetier.Creer("Cloud Azure");

    [Fact]
    public void UnAbonnementEstCreeAvecLesInformationsValides()
    {
        var abonnement = Entites.Abonnement.Creer(UtilisateurId, DomaineValide());

        abonnement.Should().NotBeNull();
        abonnement.Id.Should().NotBe(Guid.Empty);
        abonnement.UtilisateurId.Should().Be(UtilisateurId);
        abonnement.DomaineMetier.Valeur.Should().Be("Cloud Azure");
    }

    [Fact]
    public void UnNouvelAbonnementAUneDateDInscriptionRenseignee()
    {
        var avant = DateTime.UtcNow;
        var abonnement = Entites.Abonnement.Creer(UtilisateurId, DomaineValide());

        abonnement.DateInscription.Should().BeOnOrAfter(avant);
    }

    [Fact]
    public void CreerAvecUtilisateurIdVideLeveArgumentException()
    {
        var creer = () => Entites.Abonnement.Creer(Guid.Empty, DomaineValide());

        creer.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreerAvecDomaineMetierNullLeveArgumentNullException()
    {
        var creer = () => Entites.Abonnement.Creer(UtilisateurId, null!);

        creer.Should().Throw<ArgumentNullException>();
    }
}
