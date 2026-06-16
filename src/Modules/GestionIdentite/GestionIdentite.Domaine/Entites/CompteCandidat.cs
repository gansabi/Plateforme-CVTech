using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;

namespace GestionIdentite.Domaine.Entites;

public sealed class CompteCandidat
{
    // Setters private requis par EF Core pour la matérialisation.
    // En dehors de l'ORM, seul Creer() produit des instances valides.
    public Guid Id { get; private set; }
    public Email Email { get; private set; } = null!;
    public Role Role { get; private set; }
    public bool EstBloque { get; private set; }

    public string MotDePasseHache { get; private set; } = string.Empty;

    // internal : EF Core accède via réflexion ; empêche l'instanciation directe hors assembly.
    internal CompteCandidat() { }

    private CompteCandidat(Guid id, Email email, string motDePasseHache)
    {
        Id = id;
        Email = email;
        MotDePasseHache = motDePasseHache;
        Role = Role.Candidat;
        EstBloque = false;
    }

    public static CompteCandidat Creer(Email email, MotDePasse motDePasse)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(motDePasse);

        return new CompteCandidat(Guid.NewGuid(), email, string.Empty);
    }

    public static CompteCandidat CréerAvecMotDePasseHache(Email email, string motDePasseHache)
    {
        ArgumentNullException.ThrowIfNull(email);

        return new CompteCandidat(Guid.NewGuid(), email, motDePasseHache);
    }

    public void Bloquer()
    {
        if (EstBloque)
            throw new CompteDejaBloqueException();

        EstBloque = true;
    }

    public void Reactiver()
    {
        if (!EstBloque)
            throw new CompteNonBloqueException();

        EstBloque = false;
    }
}
