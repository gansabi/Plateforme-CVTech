using ActualiteEtAbonnement.Domaine.Exceptions;
using ActualiteEtAbonnement.Domaine.ObjetsValeur;

namespace ActualiteEtAbonnement.Domaine.Entites;

public sealed class ArticleActualite
{
    public Guid Id { get; private set; }
    public Guid AuteurId { get; private set; }
    public string Titre { get; private set; } = string.Empty;
    public string Contenu { get; private set; } = string.Empty;
    public DomaineMetier DomaineMetier { get; private set; } = null!;
    public DateTime DatePublication { get; private set; }

    internal ArticleActualite() { }

    private ArticleActualite(Guid id, Guid auteurId, string titre, string contenu, DomaineMetier domaineMetier)
    {
        Id = id;
        AuteurId = auteurId;
        Titre = titre;
        Contenu = contenu;
        DomaineMetier = domaineMetier;
        DatePublication = DateTime.UtcNow;
    }

    public static ArticleActualite Publier(Guid auteurId, string titre, string contenu, DomaineMetier domaineMetier)
    {
        if (auteurId == Guid.Empty)
            throw new ArgumentException("L'identifiant de l'auteur ne peut pas être vide.", nameof(auteurId));

        if (string.IsNullOrWhiteSpace(titre))
            throw new TitreArticleInvalideException("Le titre de l'article ne peut pas être vide.");

        if (string.IsNullOrWhiteSpace(contenu))
            throw new ContenuArticleInvalideException("Le contenu de l'article ne peut pas être vide.");

        ArgumentNullException.ThrowIfNull(domaineMetier);

        return new ArticleActualite(Guid.NewGuid(), auteurId, titre.Trim(), contenu.Trim(), domaineMetier);
    }
}
