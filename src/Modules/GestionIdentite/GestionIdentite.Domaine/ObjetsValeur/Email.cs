using GestionIdentite.Domaine.Exceptions;

namespace GestionIdentite.Domaine.ObjetsValeur;

public sealed class Email : IEquatable<Email>
{
    // Setter private requis par EF Core pour la matérialisation (OwnsOne).
    // La valeur n'est jamais définie directement en dehors de Creer().
    public string Valeur { get; private set; } = string.Empty;

    // Constructeur sans paramètre pour EF Core — ne doit pas être appelé
    // depuis le code métier ; toute création passe par Creer().
    private Email() { }

    private Email(string valeur) => Valeur = valeur;

    public static Email Creer(string adresse)
    {
        if (string.IsNullOrWhiteSpace(adresse))
            throw new EmailInvalideException("L'adresse email est vide.");

        var normalise = adresse.Trim().ToLowerInvariant();

        if (!EstFormatValide(normalise))
            throw new EmailInvalideException(
                $"Le format de l'adresse email '{adresse}' est invalide.");

        return new Email(normalise);
    }

    private static bool EstFormatValide(string adresse)
    {
        if (adresse.Any(char.IsWhiteSpace))
            return false;

        var positionArobase = adresse.IndexOf('@');
        if (positionArobase <= 0)
            return false;

        var domaine = adresse[(positionArobase + 1)..];
        if (string.IsNullOrEmpty(domaine))
            return false;

        var positionPoint = domaine.IndexOf('.');
        if (positionPoint <= 0)
            return false;

        if (positionPoint == domaine.Length - 1)
            return false;

        return true;
    }

    public bool Equals(Email? other) =>
        other is not null && Valeur == other.Valeur;

    public override bool Equals(object? obj) =>
        obj is Email other && Equals(other);

    public override int GetHashCode() =>
        Valeur.GetHashCode(StringComparison.Ordinal);

    public override string ToString() => Valeur;
}
