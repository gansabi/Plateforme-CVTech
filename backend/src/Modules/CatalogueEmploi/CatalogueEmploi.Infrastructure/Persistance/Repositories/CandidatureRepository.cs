using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace CatalogueEmploi.Infrastructure.Persistance.Repositories;

public sealed class CandidatureRepository : ICandidatureRepository
{
    private readonly CatalogueEmploiDbContext _dbContext;

    public CandidatureRepository(CatalogueEmploiDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(Candidature candidature, CancellationToken cancellationToken = default)
    {
        await _dbContext.Candidatures.AddAsync(candidature, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExisteDejaAsync(Guid candidatId, Guid annonceId, CancellationToken cancellationToken = default)
        => await _dbContext.Candidatures
            .AnyAsync(c => c.CandidatId == candidatId && c.AnnonceId == annonceId, cancellationToken);

    public async Task<IReadOnlyList<Candidature>> ListerParAnnonceIdAsync(Guid annonceId, CancellationToken cancellationToken = default)
        => await _dbContext.Candidatures
            .Where(c => c.AnnonceId == annonceId)
            .OrderByDescending(c => c.DatePostulation)
            .ToListAsync(cancellationToken);
}
