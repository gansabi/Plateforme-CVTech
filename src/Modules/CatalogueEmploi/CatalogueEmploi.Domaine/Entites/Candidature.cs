namespace CatalogueEmploi.Domaine.Entites;

public sealed class Candidature
{
    public Guid Id { get; private set; }
    public Guid CandidatId { get; private set; }
    public Guid AnnonceId { get; private set; }
    public DateTime DatePostulation { get; private set; }
    public string? LettreMotivation { get; private set; }

    internal Candidature() { }

    private Candidature(Guid id, Guid candidatId, Guid annonceId, string? lettreMotivation)
    {
        Id = id;
        CandidatId = candidatId;
        AnnonceId = annonceId;
        DatePostulation = DateTime.UtcNow;
        LettreMotivation = lettreMotivation;
    }

    public static Candidature Creer(Guid candidatId, Guid annonceId, string? lettreMotivation)
    {
        if (candidatId == Guid.Empty)
            throw new ArgumentException("L'identifiant du candidat ne peut pas être vide.", nameof(candidatId));

        if (annonceId == Guid.Empty)
            throw new ArgumentException("L'identifiant de l'annonce ne peut pas être vide.", nameof(annonceId));

        return new Candidature(Guid.NewGuid(), candidatId, annonceId, lettreMotivation);
    }
}
