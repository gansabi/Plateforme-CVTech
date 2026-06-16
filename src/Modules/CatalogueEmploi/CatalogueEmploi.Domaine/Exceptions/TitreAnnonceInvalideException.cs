namespace CatalogueEmploi.Domaine.Exceptions;

public sealed class TitreAnnonceInvalideException : Exception
{
    public TitreAnnonceInvalideException(string message) : base(message) { }
}
