using GestionIdentite.Domaine.Entites;

namespace GestionIdentite.Domaine.Contrats;

public interface ICompteEntrepriseRepository
{
    Task<bool> ExisteAvecEmailAsync(string email, CancellationToken cancellationToken = default);
    Task SauvegarderAsync(CompteEntreprise compte, CancellationToken cancellationToken = default);
    Task<CompteEntreprise?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CompteEntreprise?> TrouverParEmailAsync(string email, CancellationToken cancellationToken = default);
}
