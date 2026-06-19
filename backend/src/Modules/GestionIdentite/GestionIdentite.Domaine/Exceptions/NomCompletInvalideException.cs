namespace GestionIdentite.Domaine.Exceptions;

public sealed class NomCompletInvalideException : Exception
{
    public NomCompletInvalideException(string message) : base(message) { }
}
