using CatalogueEmploi.Domaine.Enums;
using CatalogueEmploi.Domaine.Exceptions;
using CatalogueEmploi.Domaine.ObjetsValeur;

namespace CatalogueEmploi.Domaine.Entites;

public sealed class AnnonceEmploi
{
    public Guid Id { get; private set; }
    public Guid EntrepriseId { get; private set; }
    public string Titre { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TypeContrat TypeContrat { get; private set; }
    public DomaineMetier DomaineMetier { get; private set; } = null!;
    public DateTime DatePublication { get; private set; }
    public bool EstActive { get; private set; }

    internal AnnonceEmploi() { }

    private AnnonceEmploi(Guid id, Guid entrepriseId, string titre, string description,
        TypeContrat typeContrat, DomaineMetier domaineMetier)
    {
        Id = id;
        EntrepriseId = entrepriseId;
        Titre = titre;
        Description = description;
        TypeContrat = typeContrat;
        DomaineMetier = domaineMetier;
        DatePublication = DateTime.UtcNow;
        EstActive = true;
    }

    public static AnnonceEmploi Publier(Guid entrepriseId, string titre, string description,
        TypeContrat typeContrat, DomaineMetier domaineMetier)
    {
        if (entrepriseId == Guid.Empty)
            throw new ArgumentException("L'identifiant de l'entreprise ne peut pas être vide.", nameof(entrepriseId));

        if (string.IsNullOrWhiteSpace(titre))
            throw new TitreAnnonceInvalideException("Le titre de l'annonce ne peut pas être vide.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DescriptionAnnonceInvalideException("La description de l'annonce ne peut pas être vide.");

        ArgumentNullException.ThrowIfNull(domaineMetier);

        return new AnnonceEmploi(Guid.NewGuid(), entrepriseId, titre.Trim(), description.Trim(),
            typeContrat, domaineMetier);
    }

    public void Moderer() => EstActive = false;

    public void Supprimer() => EstActive = false;
}
