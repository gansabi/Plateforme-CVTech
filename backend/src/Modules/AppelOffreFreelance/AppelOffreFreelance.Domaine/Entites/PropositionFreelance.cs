namespace AppelOffreFreelance.Domaine.Entites;

public sealed class PropositionFreelance
{
    public Guid Id { get; private set; }
    public Guid CandidatId { get; private set; }
    public Guid AppelOffreId { get; private set; }
    public decimal TarifJournalier { get; private set; }
    public int DureeJours { get; private set; }
    public string Methodologie { get; private set; } = string.Empty;
    public DateTime DateSoumission { get; private set; }

    internal PropositionFreelance() { }

    private PropositionFreelance(Guid id, Guid candidatId, Guid appelOffreId,
        decimal tarifJournalier, int dureeJours, string methodologie)
    {
        Id = id;
        CandidatId = candidatId;
        AppelOffreId = appelOffreId;
        TarifJournalier = tarifJournalier;
        DureeJours = dureeJours;
        Methodologie = methodologie;
        DateSoumission = DateTime.UtcNow;
    }

    public static PropositionFreelance Creer(Guid candidatId, Guid appelOffreId,
        decimal tarifJournalier, int dureeJours, string methodologie)
    {
        if (candidatId == Guid.Empty)
            throw new ArgumentException("L'identifiant du candidat ne peut pas être vide.", nameof(candidatId));

        if (appelOffreId == Guid.Empty)
            throw new ArgumentException("L'identifiant de l'appel d'offre ne peut pas être vide.", nameof(appelOffreId));

        if (tarifJournalier <= 0)
            throw new ArgumentException("Le tarif journalier doit être supérieur à zéro.", nameof(tarifJournalier));

        if (dureeJours <= 0)
            throw new ArgumentException("La durée doit être supérieure à zéro jours.", nameof(dureeJours));

        return new PropositionFreelance(Guid.NewGuid(), candidatId, appelOffreId,
            tarifJournalier, dureeJours, methodologie ?? string.Empty);
    }
}
