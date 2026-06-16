using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using AppelOffreFreelance.Domaine.Enums;
using Microsoft.EntityFrameworkCore;

namespace AppelOffreFreelance.Infrastructure.Persistance.Repositories;

public sealed class AppelOffreRepository : IAppelOffreRepository
{
    private readonly AppelOffreFreelanceDbContext _dbContext;

    public AppelOffreRepository(AppelOffreFreelanceDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(AppelOffre appelOffre, CancellationToken cancellationToken = default)
    {
        await _dbContext.AppelsOffre.AddAsync(appelOffre, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<AppelOffre?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.AppelsOffre.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<AppelOffre>> ListerOuvertsAsync(CancellationToken cancellationToken = default)
        => await _dbContext.AppelsOffre
            .Where(a => a.Statut == StatutAppelOffre.Ouvert)
            .OrderByDescending(a => a.DatePublication)
            .ToListAsync(cancellationToken);

    public async Task MettreAJourAsync(AppelOffre appelOffre, CancellationToken cancellationToken = default)
    {
        _dbContext.AppelsOffre.Update(appelOffre);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
