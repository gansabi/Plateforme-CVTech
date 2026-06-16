using AppelOffreFreelance.Domaine.Entites;

namespace AppelOffreFreelance.Domaine.Contrats;

public interface IAppelOffreRepository
{
    Task SauvegarderAsync(AppelOffre appelOffre, CancellationToken cancellationToken = default);
    Task<AppelOffre?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AppelOffre>> ListerOuvertsAsync(CancellationToken cancellationToken = default);
    Task MettreAJourAsync(AppelOffre appelOffre, CancellationToken cancellationToken = default);
}
