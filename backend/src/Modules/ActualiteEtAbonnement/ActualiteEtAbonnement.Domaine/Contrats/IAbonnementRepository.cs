using ActualiteEtAbonnement.Domaine.Entites;

namespace ActualiteEtAbonnement.Domaine.Contrats;

public interface IAbonnementRepository
{
    Task SauvegarderAsync(Abonnement abonnement, CancellationToken cancellationToken = default);
    Task<bool> ExisteDejaAsync(Guid utilisateurId, string domaineMetier, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Abonnement>> ListerParDomaineAsync(string domaineMetier, CancellationToken cancellationToken = default);
}
