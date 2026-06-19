using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using GestionIdentite.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GestionIdentite.Infrastructure.Persistance.Repositories;

public sealed class CompteRepository : ICompteRepository
{
    private readonly GestionIdentiteDbContext _dbContext;

    public CompteRepository(GestionIdentiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExisteAvecEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var emailNormalise = email.ToLowerInvariant();

        return await _dbContext.ComptesCandidats
            .AnyAsync(c => c.Email.Valeur == emailNormalise, cancellationToken);
    }

    public async Task SauvegarderAsync(
        CompteCandidat compte,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ComptesCandidats.AddAsync(compte, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CompteCandidat?> TrouverParIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await _dbContext.ComptesCandidats.FindAsync([id], cancellationToken);
}
