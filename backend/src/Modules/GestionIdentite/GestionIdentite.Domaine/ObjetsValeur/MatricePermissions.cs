using GestionIdentite.Domaine.Enums;

namespace GestionIdentite.Domaine.ObjetsValeur;

/// <summary>
/// Source de vérité du mappage Rôle → ensemble de permissions.
/// Traduit la matrice définie dans .agent/skills/regles-permissions.md.
/// </summary>
public static class MatricePermissions
{
    private static readonly IReadOnlyDictionary<Role, IReadOnlySet<Permission>> Matrice =
        new Dictionary<Role, IReadOnlySet<Permission>>
        {
            [Role.Candidat] = new HashSet<Permission>
            {
                Permission.CreerCurriculumVitae,
                Permission.ModifierCurriculumVitae,
                Permission.PostulerAnnonce,
                Permission.SoumettrePropositon,
                Permission.SAbonnerDomaine
            },
            [Role.Entreprise] = new HashSet<Permission>
            {
                Permission.PublierAnnonce,
                Permission.PublierAppelOffre,
                Permission.ConsulterCandidaturesRecues,
                Permission.ConsulterPropositionsRecues,
                Permission.SAbonnerDomaine
            },
            [Role.Administrateur] = new HashSet<Permission>
            {
                // Droits hérités Candidat (sauf PostulerAnnonce et SoumettrePropositon)
                Permission.CreerCurriculumVitae,
                Permission.ModifierCurriculumVitae,
                Permission.SAbonnerDomaine,
                // Droits hérités Entreprise
                Permission.PublierAnnonce,
                Permission.PublierAppelOffre,
                Permission.ConsulterCandidaturesRecues,
                Permission.ConsulterPropositionsRecues,
                // Droits exclusifs Administrateur
                Permission.PublierArticleActualite,
                Permission.ModererAnnonce,
                Permission.SupprimerAnnonce,
                Permission.ModererAppelOffre,
                Permission.SupprimerAppelOffre,
                Permission.BloquerCompte,
                Permission.ReactiverCompte,
                Permission.GererReferentielDomaineMetier
            }
        };

    public static bool PossedePermission(Role role, Permission permission)
        => Matrice.TryGetValue(role, out var permissions) && permissions.Contains(permission);

    public static IReadOnlySet<Permission> ObtenirPermissions(Role role)
        => Matrice.TryGetValue(role, out var permissions)
            ? permissions
            : new HashSet<Permission>();
}
