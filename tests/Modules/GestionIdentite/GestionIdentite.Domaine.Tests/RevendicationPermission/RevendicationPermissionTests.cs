using FluentAssertions;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.ObjetsValeur;
using Xunit;

namespace GestionIdentite.Domaine.Tests.RevendicationPermission;

public sealed class RevendicationPermissionTests
{
    [Fact]
    public void UneRevendicationCandidatContientSesPermissions()
    {
        var revendication = new Domaine.ObjetsValeur.RevendicationPermission(Guid.NewGuid(), Role.Candidat);

        revendication.Permissions.Should().Contain(Permission.PostulerAnnonce);
        revendication.Permissions.Should().Contain(Permission.CreerCurriculumVitae);
    }

    [Fact]
    public void UneRevendicationAdministrateurPossedePermissionBloquerCompte()
    {
        var revendication = new Domaine.ObjetsValeur.RevendicationPermission(Guid.NewGuid(), Role.Administrateur);

        revendication.PossedePermission(Permission.BloquerCompte).Should().BeTrue();
    }

    [Fact]
    public void UneRevendicationCandidatNePossedesPasBloquerCompte()
    {
        var revendication = new Domaine.ObjetsValeur.RevendicationPermission(Guid.NewGuid(), Role.Candidat);

        revendication.PossedePermission(Permission.BloquerCompte).Should().BeFalse();
    }

    [Fact]
    public void UneRevendicationAvecIdVideLeveUneException()
    {
        var creer = () => new Domaine.ObjetsValeur.RevendicationPermission(Guid.Empty, Role.Candidat);

        creer.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UneRevendicationContientLIdentifiantUtilisateur()
    {
        var id = Guid.NewGuid();
        var revendication = new Domaine.ObjetsValeur.RevendicationPermission(id, Role.Entreprise);

        revendication.UtilisateurId.Should().Be(id);
        revendication.Role.Should().Be(Role.Entreprise);
    }
}
