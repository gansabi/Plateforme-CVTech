namespace GestionIdentite.Domaine.Exceptions;

public sealed class EmailInvalideException : Exception
{
    public EmailInvalideException(string message) : base(message) { }
}
