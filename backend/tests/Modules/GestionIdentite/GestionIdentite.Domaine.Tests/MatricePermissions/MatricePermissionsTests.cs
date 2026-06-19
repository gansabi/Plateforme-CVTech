using FluentAssertions;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.ObjetsValeur;
using Xunit;

namespace GestionIdentite.Domaine.Tests.MatricePermissions;

public sealed class MatricePermissionsTests
{
    // -----------------------------------------------------------------------
    // Candidat
    // -----------------------------------------------------------------------

    [Fact]
    public void UnCandidatPeutCreerSonCurriculumVitae()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Candidat, Permission.CreerCurriculumVitae)
            .Should().BeTrue();

    [Fact]
    public void UnCandidatPeutModifierSonCurriculumVitae()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Candidat, Permission.ModifierCurriculumVitae)
            .Should().BeTrue();

    [Fact]
    public void UnCandidatPeutPostulerAUneAnnonce()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Candidat, Permission.PostulerAnnonce)
            .Should().BeTrue();

    [Fact]
    public void UnCandidatPeutSoumettreUneProposition()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Candidat, Permission.SoumettrePropositon)
            .Should().BeTrue();

    [Fact]
    public void UnCandidatNePeutPasPublierUneAnnonce()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Candidat, Permission.PublierAnnonce)
            .Should().BeFalse();

    [Fact]
    public void UnCandidatNePeutPasBloquerUnCompte()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Candidat, Permission.BloquerCompte)
            .Should().BeFalse();

    // -----------------------------------------------------------------------
    // Entreprise
    // -----------------------------------------------------------------------

    [Fact]
    public void UneEntreprisePeutPublierUneAnnonce()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Entreprise, Permission.PublierAnnonce)
            .Should().BeTrue();

    [Fact]
    public void UneEntreprisePeutConsulterLesCandidaturesRecues()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Entreprise, Permission.ConsulterCandidaturesRecues)
            .Should().BeTrue();

    [Fact]
    public void UneEntrepriseNePeutPasPostulerAUneAnnonce()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Entreprise, Permission.PostulerAnnonce)
            .Should().BeFalse();

    [Fact]
    public void UneEntrepriseNePeutPasBloquerUnCompte()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Entreprise, Permission.BloquerCompte)
            .Should().BeFalse();

    // -----------------------------------------------------------------------
    // Administrateur
    // -----------------------------------------------------------------------

    [Fact]
    public void UnAdministrateurPeutBloquerUnCompte()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Administrateur, Permission.BloquerCompte)
            .Should().BeTrue();

    [Fact]
    public void UnAdministrateurPeutReactiverUnCompte()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Administrateur, Permission.ReactiverCompte)
            .Should().BeTrue();

    [Fact]
    public void UnAdministrateurPeutPublierUnArticle()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Administrateur, Permission.PublierArticleActualite)
            .Should().BeTrue();

    [Fact]
    public void UnAdministrateurNePeutPasPostulerAUneAnnonce()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Administrateur, Permission.PostulerAnnonce)
            .Should().BeFalse();

    // -----------------------------------------------------------------------
    // ObtenirPermissions
    // -----------------------------------------------------------------------

    [Fact]
    public void LesPermissionsDeCandidatContiennentCinqElements()
        => Domaine.ObjetsValeur.MatricePermissions
            .ObtenirPermissions(Role.Candidat)
            .Should().HaveCount(5);

    [Fact]
    public void LesPermissionsEntrepriseContiennentCinqElements()
        => Domaine.ObjetsValeur.MatricePermissions
            .ObtenirPermissions(Role.Entreprise)
            .Should().HaveCount(5);

    [Fact]
    public void LesPermissionsAdministrateurContiennentQuinzeElements()
        => Domaine.ObjetsValeur.MatricePermissions
            .ObtenirPermissions(Role.Administrateur)
            .Should().HaveCount(15);

    [Fact]
    public void UnAdministrateurPeutCreerUnCurriculumVitae()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Administrateur, Permission.CreerCurriculumVitae)
            .Should().BeTrue();

    [Fact]
    public void UnAdministrateurPeutPublierUneAnnonce()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Administrateur, Permission.PublierAnnonce)
            .Should().BeTrue();

    [Fact]
    public void UnAdministrateurNePeutPasSoumettreUneProposition()
        => Domaine.ObjetsValeur.MatricePermissions
            .PossedePermission(Role.Administrateur, Permission.SoumettrePropositon)
            .Should().BeFalse();
}
