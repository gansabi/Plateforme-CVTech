namespace GestionIdentite.Domaine.Exceptions;

public sealed class CompteDejaBloqueException : Exception
{
    public CompteDejaBloqueException()
        : base("Le compte est déjà bloqué.") { }
}
