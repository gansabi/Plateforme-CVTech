using ActualiteEtAbonnement.Domaine.Contrats;
using ActualiteEtAbonnement.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace ActualiteEtAbonnement.Infrastructure.Persistance.Repositories;

public sealed class AbonnementRepository : IAbonnementRepository
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;

    public AbonnementRepository(ActualiteEtAbonnementDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(Abonnement abonnement, CancellationToken cancellationToken = default)
    {
        await _dbContext.Abonnements.AddAsync(abonnement, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExisteDejaAsync(Guid utilisateurId, string domaineMetier, CancellationToken cancellationToken = default)
        => await _dbContext.Abonnements
            .AnyAsync(a => a.UtilisateurId == utilisateurId && a.DomaineMetier.Valeur == domaineMetier, cancellationToken);

    public async Task<IReadOnlyList<Abonnement>> ListerParDomaineAsync(string domaineMetier, CancellationToken cancellationToken = default)
        => await _dbContext.Abonnements
            .Where(a => a.DomaineMetier.Valeur == domaineMetier)
            .ToListAsync(cancellationToken);
}
