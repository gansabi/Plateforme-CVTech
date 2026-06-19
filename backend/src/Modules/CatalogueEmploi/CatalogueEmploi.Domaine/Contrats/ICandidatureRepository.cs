using CatalogueEmploi.Domaine.Entites;

namespace CatalogueEmploi.Domaine.Contrats;

public interface ICandidatureRepository
{
    Task SauvegarderAsync(Candidature candidature, CancellationToken cancellationToken = default);
    Task<bool> ExisteDejaAsync(Guid candidatId, Guid annonceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Candidature>> ListerParAnnonceIdAsync(Guid annonceId, CancellationToken cancellationToken = default);
}
