namespace CatalogueEmploi.Domaine.Exceptions;

public sealed class AnnonceNonTrouveeException : Exception
{
    public AnnonceNonTrouveeException(Guid annonceId)
        : base($"L'annonce '{annonceId}' est introuvable.") { }
}
