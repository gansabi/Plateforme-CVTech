using ActualiteEtAbonnement.Domaine.Enums;

namespace ActualiteEtAbonnement.Domaine.Entites;

public sealed class Notification
{
    public Guid Id { get; private set; }
    public Guid DestinataireId { get; private set; }
    public string Titre { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public CanalDiffusion Canal { get; private set; }
    public DateTime DateCreation { get; private set; }
    public bool EstLue { get; private set; }

    internal Notification() { }

    private Notification(Guid id, Guid destinataireId, string titre, string message, CanalDiffusion canal)
    {
        Id = id;
        DestinataireId = destinataireId;
        Titre = titre;
        Message = message;
        Canal = canal;
        DateCreation = DateTime.UtcNow;
        EstLue = false;
    }

    public static Notification Creer(Guid destinataireId, string titre, string message, CanalDiffusion canal)
    {
        if (destinataireId == Guid.Empty)
            throw new ArgumentException("L'identifiant du destinataire ne peut pas être vide.", nameof(destinataireId));

        return new Notification(Guid.NewGuid(), destinataireId, titre ?? string.Empty, message ?? string.Empty, canal);
    }

    public void MarquerCommeLue() => EstLue = true;
}
