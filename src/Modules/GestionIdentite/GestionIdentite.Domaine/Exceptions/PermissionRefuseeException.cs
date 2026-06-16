namespace GestionIdentite.Domaine.Exceptions;

public sealed class PermissionRefuseeException : Exception
{
    public PermissionRefuseeException(string message) : base(message) { }
}
