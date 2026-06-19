namespace GestionIdentite.Domaine.Exceptions;

public sealed class EmailDejaUtiliseException : Exception
{
    public EmailDejaUtiliseException(string email)
        : base($"Un compte avec l'adresse email '{email}' existe déjà.") { }
}
