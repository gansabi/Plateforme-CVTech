using AppelOffreFreelance.Domaine.Entites;

namespace AppelOffreFreelance.Domaine.Contrats;

public interface IPropositionFreelanceRepository
{
    Task SauvegarderAsync(PropositionFreelance proposition, CancellationToken cancellationToken = default);
    Task<bool> ExisteDejaAsync(Guid candidatId, Guid appelOffreId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PropositionFreelance>> ListerParAppelOffreIdAsync(Guid appelOffreId, CancellationToken cancellationToken = default);
}
