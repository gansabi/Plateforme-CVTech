using CatalogueEmploi.Domaine.Contrats;
using CatalogueEmploi.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace CatalogueEmploi.Infrastructure.Persistance.Repositories;

public sealed class CurriculumVitaeRepository : ICurriculumVitaeRepository
{
    private readonly CatalogueEmploiDbContext _dbContext;

    public CurriculumVitaeRepository(CatalogueEmploiDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(CurriculumVitae cv, CancellationToken cancellationToken = default)
    {
        await _dbContext.CurriculumsVitae.AddAsync(cv, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CurriculumVitae?> TrouverParCandidatIdAsync(Guid candidatId, CancellationToken cancellationToken = default)
        => await _dbContext.CurriculumsVitae
            .FirstOrDefaultAsync(cv => cv.CandidatId == candidatId, cancellationToken);

    public async Task MettreAJourAsync(CurriculumVitae cv, CancellationToken cancellationToken = default)
    {
        _dbContext.CurriculumsVitae.Update(cv);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
