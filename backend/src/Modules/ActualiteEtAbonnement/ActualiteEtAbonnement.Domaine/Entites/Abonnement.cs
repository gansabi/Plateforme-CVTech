using ActualiteEtAbonnement.Domaine.ObjetsValeur;

namespace ActualiteEtAbonnement.Domaine.Entites;

public sealed class Abonnement
{
    public Guid Id { get; private set; }
    public Guid UtilisateurId { get; private set; }
    public DomaineMetier DomaineMetier { get; private set; } = null!;
    public DateTime DateInscription { get; private set; }

    internal Abonnement() { }

    private Abonnement(Guid id, Guid utilisateurId, DomaineMetier domaineMetier)
    {
        Id = id;
        UtilisateurId = utilisateurId;
        DomaineMetier = domaineMetier;
        DateInscription = DateTime.UtcNow;
    }

    public static Abonnement Creer(Guid utilisateurId, DomaineMetier domaineMetier)
    {
        if (utilisateurId == Guid.Empty)
            throw new ArgumentException("L'identifiant de l'utilisateur ne peut pas être vide.", nameof(utilisateurId));

        ArgumentNullException.ThrowIfNull(domaineMetier);

        return new Abonnement(Guid.NewGuid(), utilisateurId, domaineMetier);
    }
}
