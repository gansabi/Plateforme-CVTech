using AppelOffreFreelance.Domaine.Contrats;
using AppelOffreFreelance.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace AppelOffreFreelance.Infrastructure.Persistance.Repositories;

public sealed class PropositionFreelanceRepository : IPropositionFreelanceRepository
{
    private readonly AppelOffreFreelanceDbContext _dbContext;

    public PropositionFreelanceRepository(AppelOffreFreelanceDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SauvegarderAsync(PropositionFreelance proposition, CancellationToken cancellationToken = default)
    {
        await _dbContext.Propositions.AddAsync(proposition, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExisteDejaAsync(Guid candidatId, Guid appelOffreId, CancellationToken cancellationToken = default)
        => await _dbContext.Propositions
            .AnyAsync(p => p.CandidatId == candidatId && p.AppelOffreId == appelOffreId, cancellationToken);

    public async Task<IReadOnlyList<PropositionFreelance>> ListerParAppelOffreIdAsync(Guid appelOffreId, CancellationToken cancellationToken = default)
        => await _dbContext.Propositions
            .Where(p => p.AppelOffreId == appelOffreId)
            .OrderByDescending(p => p.DateSoumission)
            .ToListAsync(cancellationToken);
}
