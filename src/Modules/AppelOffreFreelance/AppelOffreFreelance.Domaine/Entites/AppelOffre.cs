using AppelOffreFreelance.Domaine.Enums;
using AppelOffreFreelance.Domaine.Exceptions;
using AppelOffreFreelance.Domaine.ObjetsValeur;

namespace AppelOffreFreelance.Domaine.Entites;

public sealed class AppelOffre
{
    public Guid Id { get; private set; }
    public Guid EntrepriseId { get; private set; }
    public string Titre { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DomaineMetier DomaineMetier { get; private set; } = null!;
    public BaremeTJM BaremeTJM { get; private set; } = null!;
    public DateTime DateLimite { get; private set; }
    public DateTime DatePublication { get; private set; }
    public StatutAppelOffre Statut { get; private set; }

    internal AppelOffre() { }

    private AppelOffre(Guid id, Guid entrepriseId, string titre, string description,
        DomaineMetier domaineMetier, BaremeTJM baremeTjm, DateTime dateLimite)
    {
        Id = id;
        EntrepriseId = entrepriseId;
        Titre = titre;
        Description = description;
        DomaineMetier = domaineMetier;
        BaremeTJM = baremeTjm;
        DateLimite = dateLimite;
        DatePublication = DateTime.UtcNow;
        Statut = StatutAppelOffre.Ouvert;
    }

    public static AppelOffre Publier(Guid entrepriseId, string titre, string description,
        DomaineMetier domaineMetier, BaremeTJM baremeTjm, DateTime dateLimite)
    {
        if (entrepriseId == Guid.Empty)
            throw new ArgumentException("L'identifiant de l'entreprise ne peut pas être vide.", nameof(entrepriseId));

        if (string.IsNullOrWhiteSpace(titre))
            throw new TitreAppelOffreInvalideException("Le titre de l'appel d'offre ne peut pas être vide.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DescriptionAppelOffreInvalideException("La description de l'appel d'offre ne peut pas être vide.");

        ArgumentNullException.ThrowIfNull(domaineMetier);
        ArgumentNullException.ThrowIfNull(baremeTjm);

        return new AppelOffre(Guid.NewGuid(), entrepriseId, titre.Trim(), description.Trim(),
            domaineMetier, baremeTjm, dateLimite);
    }

    public void Moderer() => Statut = StatutAppelOffre.Ferme;

    public void Supprimer() => Statut = StatutAppelOffre.Ferme;
}
