namespace CatalogueEmploi.Domaine.Exceptions;

public sealed class TitreCVInvalideException : Exception
{
    public TitreCVInvalideException(string message) : base(message) { }
}
