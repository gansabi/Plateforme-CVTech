namespace GestionIdentite.Domaine.Exceptions;

public sealed class MotDePasseInvalideException : Exception
{
    public MotDePasseInvalideException(string message) : base(message) { }
}
