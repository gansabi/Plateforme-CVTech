using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace GestionIdentite.Infrastructure.Persistance.Repositories;

public sealed class CompteAdministrateurRepository : ICompteAdministrateurRepository
{
    private readonly GestionIdentiteDbContext _dbContext;

    public CompteAdministrateurRepository(GestionIdentiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExisteAvecEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var emailNormalise = email.ToLowerInvariant();
        return await _dbContext.ComptesAdministrateurs
            .AnyAsync(c => c.Email.Valeur == emailNormalise, cancellationToken);
    }

    public async Task SauvegarderAsync(CompteAdministrateur compte, CancellationToken cancellationToken = default)
    {
        _dbContext.ComptesAdministrateurs.Add(compte);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CompteAdministrateur?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.ComptesAdministrateurs.FindAsync([id], cancellationToken);
}
