using CatalogueEmploi.Domaine.Entites;

namespace CatalogueEmploi.Domaine.Contrats;

public interface IAnnonceEmploiRepository
{
    Task SauvegarderAsync(AnnonceEmploi annonce, CancellationToken cancellationToken = default);
    Task<AnnonceEmploi?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AnnonceEmploi>> ListerActivesAsync(CancellationToken cancellationToken = default);
    Task MettreAJourAsync(AnnonceEmploi annonce, CancellationToken cancellationToken = default);
}
