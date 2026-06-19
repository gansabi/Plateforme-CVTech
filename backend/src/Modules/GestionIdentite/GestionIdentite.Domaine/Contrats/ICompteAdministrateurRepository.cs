using GestionIdentite.Domaine.Entites;

namespace GestionIdentite.Domaine.Contrats;

public interface ICompteAdministrateurRepository
{
    Task<bool> ExisteAvecEmailAsync(string email, CancellationToken cancellationToken = default);
    Task SauvegarderAsync(CompteAdministrateur compte, CancellationToken cancellationToken = default);
    Task<CompteAdministrateur?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default);
}
