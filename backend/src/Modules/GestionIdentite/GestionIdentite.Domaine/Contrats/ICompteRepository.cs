using GestionIdentite.Domaine.Entites;

namespace GestionIdentite.Domaine.Contrats;

public interface ICompteRepository
{
    Task<bool> ExisteAvecEmailAsync(string email, CancellationToken cancellationToken = default);
    Task SauvegarderAsync(CompteCandidat compte, CancellationToken cancellationToken = default);
    Task<CompteCandidat?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default);
}
