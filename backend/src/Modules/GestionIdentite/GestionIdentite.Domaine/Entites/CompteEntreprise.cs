using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;

namespace GestionIdentite.Domaine.Entites;

public sealed class CompteEntreprise
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; } = null!;
    public string NomEntreprise { get; private set; } = string.Empty;
    public string MotDePasseHache { get; private set; } = string.Empty;
    public Role Role { get; private set; }
    public bool EstBloque { get; private set; }

    internal CompteEntreprise() { }

    private CompteEntreprise(Guid id, Email email, string nomEntreprise, string motDePasseHache)
    {
        Id = id;
        Email = email;
        NomEntreprise = nomEntreprise;
        MotDePasseHache = motDePasseHache;
        Role = Role.Entreprise;
        EstBloque = false;
    }

    public static CompteEntreprise Creer(Email email, string nomEntreprise, string motDePasseHache)
    {
        ArgumentNullException.ThrowIfNull(email);

        if (string.IsNullOrWhiteSpace(nomEntreprise))
            throw new NomEntrepriseInvalideException("Le nom d'entreprise ne peut pas être vide.");

        return new CompteEntreprise(Guid.NewGuid(), email, nomEntreprise.Trim(), motDePasseHache);
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
