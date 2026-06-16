using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace CatalogueEmploi.Infrastructure.Persistance.Repositories;

public sealed class AnnonceEmploiRepository : IAnnonceEmploiRepository
{
    private readonly CatalogueEmploiDbContext _dbContext;

    public AnnonceEmploiRepository(CatalogueEmploiDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(AnnonceEmploi annonce, CancellationToken cancellationToken = default)
    {
        await _dbContext.AnnoncesEmploi.AddAsync(annonce, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<AnnonceEmploi?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.AnnoncesEmploi.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<AnnonceEmploi>> ListerActivesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.AnnoncesEmploi
            .Where(a => a.EstActive)
            .OrderByDescending(a => a.DatePublication)
            .ToListAsync(cancellationToken);

    public async Task MettreAJourAsync(AnnonceEmploi annonce, CancellationToken cancellationToken = default)
    {
        _dbContext.AnnoncesEmploi.Update(annonce);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
