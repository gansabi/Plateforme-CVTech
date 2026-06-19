using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace GestionIdentite.Infrastructure.Persistance.Repositories;

public sealed class CompteEntrepriseRepository : ICompteEntrepriseRepository
{
    private readonly GestionIdentiteDbContext _dbContext;

    public CompteEntrepriseRepository(GestionIdentiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExisteAvecEmailAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var emailNormalise = email.ToLowerInvariant();
        return await _dbContext.ComptesEntreprises
            .AnyAsync(c => c.Email.Valeur == emailNormalise, cancellationToken);
    }

    public async Task SauvegarderAsync(
        CompteEntreprise compte, CancellationToken cancellationToken = default)
    {
        await _dbContext.ComptesEntreprises.AddAsync(compte, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CompteEntreprise?> TrouverParIdAsync(
        Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.ComptesEntreprises.FindAsync([id], cancellationToken);

    public async Task<CompteEntreprise?> TrouverParEmailAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var emailNormalise = email.ToLowerInvariant();
        return await _dbContext.ComptesEntreprises
            .FirstOrDefaultAsync(c => c.Email.Valeur == emailNormalise, cancellationToken);
    }
}
