using CatalogueEmploi.Domaine.Entites;

namespace CatalogueEmploi.Domaine.Contrats;

public interface ICurriculumVitaeRepository
{
    Task SauvegarderAsync(CurriculumVitae cv, CancellationToken cancellationToken = default);
    Task<CurriculumVitae?> TrouverParCandidatIdAsync(Guid candidatId, CancellationToken cancellationToken = default);
    Task MettreAJourAsync(CurriculumVitae cv, CancellationToken cancellationToken = default);
}
