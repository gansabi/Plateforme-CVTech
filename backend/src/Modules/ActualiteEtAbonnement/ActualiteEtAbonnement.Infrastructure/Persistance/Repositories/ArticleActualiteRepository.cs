using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace ActualiteEtAbonnement.Infrastructure.Persistance.Repositories;

public sealed class ArticleActualiteRepository : IArticleActualiteRepository
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;

    public ArticleActualiteRepository(ActualiteEtAbonnementDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(ArticleActualite article, CancellationToken cancellationToken = default)
    {
        await _dbContext.Articles.AddAsync(article, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ArticleActualite>> ListerAsync(string? domaineMetier = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Articles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(domaineMetier))
            query = query.Where(a => a.DomaineMetier.Valeur == domaineMetier);

        return await query
            .OrderByDescending(a => a.DatePublication)
            .ToListAsync(cancellationToken);
    }
}
