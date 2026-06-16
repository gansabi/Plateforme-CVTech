namespace GestionIdentite.Domaine.Enums;

/// <summary>
/// Matrice de permissions issue de .agent/skills/regles-permissions.md
/// </summary>
public enum Permission
{
    // --- Candidat ---
    CreerCurriculumVitae,
    ModifierCurriculumVitae,
    PostulerAnnonce,
    SoumettrePropositon,
    SAbonnerDomaine,

    // --- Entreprise ---
    PublierAnnonce,
    PublierAppelOffre,
    ConsulterCandidaturesRecues,
    ConsulterPropositionsRecues,

    // --- Administrateur ---
    PublierArticleActualite,
    ModererAnnonce,
    SupprimerAnnonce,
    ModererAppelOffre,
    SupprimerAppelOffre,
    BloquerCompte,
    ReactiverCompte,
    GererReferentielDomaineMetier
}
