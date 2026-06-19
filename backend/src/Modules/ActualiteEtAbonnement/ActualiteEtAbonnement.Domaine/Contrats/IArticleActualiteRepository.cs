using ActualiteEtAbonnement.Domaine.Entites;

namespace ActualiteEtAbonnement.Domaine.Contrats;

public interface IArticleActualiteRepository
{
    Task SauvegarderAsync(ArticleActualite article, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ArticleActualite>> ListerAsync(string? domaineMetier = null, CancellationToken cancellationToken = default);
}
