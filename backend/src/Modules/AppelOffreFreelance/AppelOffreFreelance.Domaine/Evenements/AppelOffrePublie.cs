namespace AppelOffreFreelance.Domaine.Evenements;

public sealed record AppelOffrePublie(
    Guid AppelOffreId,
    Guid EntrepriseId,
    string Titre,
    string DomaineMetier,
    DateTime DatePublication);
