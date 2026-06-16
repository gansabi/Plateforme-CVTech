namespace CatalogueEmploi.Domaine.Exceptions;

public sealed class CandidatureDejaExistanteException : Exception
{
    public CandidatureDejaExistanteException()
        : base("Le candidat a déjà postulé à cette annonce.") { }
}
