namespace GestionIdentite.Domaine.Exceptions;

public sealed class CompteNonBloqueException : Exception
{
    public CompteNonBloqueException()
        : base("Le compte n'est pas bloqué et ne peut pas être réactivé.") { }
}
