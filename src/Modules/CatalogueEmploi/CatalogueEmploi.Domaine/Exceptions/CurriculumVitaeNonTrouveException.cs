namespace CatalogueEmploi.Domaine.Exceptions;

public sealed class CurriculumVitaeNonTrouveException : Exception
{
    public CurriculumVitaeNonTrouveException(Guid candidatId)
        : base($"Aucun curriculum vitae trouvé pour le candidat '{candidatId}'.") { }
}
