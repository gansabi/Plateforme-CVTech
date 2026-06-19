using ActualiteEtAbonnement.Domaine.Exceptions;

namespace ActualiteEtAbonnement.Domaine.ObjetsValeur;

public sealed class DomaineMetier
{
    public string Valeur { get; }

    private DomaineMetier(string valeur) => Valeur = valeur;

    public static DomaineMetier Creer(string valeur)
    {
        if (string.IsNullOrWhiteSpace(valeur))
            throw new DomaineMetierInvalideException("Le nom du domaine métier ne peut pas être vide.");

        if (valeur.Trim().Length > 100)
            throw new DomaineMetierInvalideException("Le nom du domaine métier ne peut pas dépasser 100 caractères.");

        return new DomaineMetier(valeur.Trim());
    }

    public override string ToString() => Valeur;
    public override bool Equals(object? obj) => obj is DomaineMetier other && Valeur == other.Valeur;
    public override int GetHashCode() => Valeur.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
