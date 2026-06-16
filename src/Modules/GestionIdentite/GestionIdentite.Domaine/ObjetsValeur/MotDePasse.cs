using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Exceptions;

namespace GestionIdentite.Domaine.ObjetsValeur;

public sealed class MotDePasse
{
    private const int LongueurMinimale = 8;

    // La valeur brute est stockée de façon interne uniquement.
    // L'infrastructure en aura besoin pour le hachage, mais elle ne doit
    // jamais apparaître dans les logs (cf. ToString ci-dessous).
    internal string Valeur { get; }

    private MotDePasse(string valeur) => Valeur = valeur;

    public static MotDePasse Creer(string valeur)
    {
        if (string.IsNullOrWhiteSpace(valeur))
            throw new MotDePasseInvalideException("Le mot de passe est vide.");

        if (valeur.Length < LongueurMinimale)
            throw new MotDePasseInvalideException(
                $"Le mot de passe doit contenir au moins {LongueurMinimale} caractères.");

        return new MotDePasse(valeur);
    }

    public string Hacher(IHasheurMotDePasse hasheur)
    {
        ArgumentNullException.ThrowIfNull(hasheur);
        return hasheur.Hacher(Valeur);
    }

    public override string ToString() => "***";
}
