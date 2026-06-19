using CatalogueEmploi.Domaine.Exceptions;

namespace CatalogueEmploi.Domaine.Entites;

public sealed class CurriculumVitae
{
    public Guid Id { get; private set; }
    public Guid CandidatId { get; private set; }
    public string Titre { get; private set; } = string.Empty;
    public string Resume { get; private set; } = string.Empty;
    public IReadOnlyList<string> CompetencesPrincipales { get; private set; } = [];
    public DateTime DateCreation { get; private set; }
    public DateTime DateMiseAJour { get; private set; }

    internal CurriculumVitae() { }

    private CurriculumVitae(Guid id, Guid candidatId, string titre, string resume,
        IEnumerable<string> competences)
    {
        Id = id;
        CandidatId = candidatId;
        Titre = titre;
        Resume = resume;
        CompetencesPrincipales = competences.ToList().AsReadOnly();
        DateCreation = DateTime.UtcNow;
        DateMiseAJour = DateCreation;
    }

    public static CurriculumVitae Creer(Guid candidatId, string titre, string resume,
        IEnumerable<string> competences)
    {
        if (candidatId == Guid.Empty)
            throw new ArgumentException("L'identifiant du candidat ne peut pas être vide.", nameof(candidatId));

        if (string.IsNullOrWhiteSpace(titre))
            throw new TitreCVInvalideException("Le titre du curriculum vitae ne peut pas être vide.");

        return new CurriculumVitae(Guid.NewGuid(), candidatId, titre.Trim(),
            resume ?? string.Empty, competences ?? []);
    }

    public void Modifier(string titre, string resume, IEnumerable<string> competences)
    {
        if (string.IsNullOrWhiteSpace(titre))
            throw new TitreCVInvalideException("Le titre du curriculum vitae ne peut pas être vide.");

        Titre = titre.Trim();
        Resume = resume ?? string.Empty;
        CompetencesPrincipales = (competences ?? []).ToList().AsReadOnly();
        DateMiseAJour = DateTime.UtcNow;
    }
}
