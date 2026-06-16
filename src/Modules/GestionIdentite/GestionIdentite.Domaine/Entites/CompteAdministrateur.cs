using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.Exceptions;
using GestionIdentite.Domaine.ObjetsValeur;

namespace GestionIdentite.Domaine.Entites;

public sealed class CompteAdministrateur
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; } = null!;
    public string NomComplet { get; private set; } = string.Empty;
    public string MotDePasseHache { get; private set; } = string.Empty;
    public Role Role { get; private set; }
    public bool EstBloque { get; private set; }

    internal CompteAdministrateur() { }

    private CompteAdministrateur(Guid id, Email email, string nomComplet, string motDePasseHache)
    {
        Id = id;
        Email = email;
        NomComplet = nomComplet;
        MotDePasseHache = motDePasseHache;
        Role = Role.Administrateur;
        EstBloque = false;
    }

    public static CompteAdministrateur Creer(Email email, string nomComplet, string motDePasseHache)
    {
        ArgumentNullException.ThrowIfNull(email);

        if (string.IsNullOrWhiteSpace(nomComplet))
            throw new NomCompletInvalideException("Le nom complet de l'administrateur ne peut pas être vide.");

        return new CompteAdministrateur(Guid.NewGuid(), email, nomComplet.Trim(), motDePasseHache);
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
