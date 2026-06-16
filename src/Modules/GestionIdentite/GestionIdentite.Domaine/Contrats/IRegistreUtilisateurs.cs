using GestionIdentite.Domaine.ObjetsValeur;

namespace GestionIdentite.Domaine.Contrats;

/// <summary>
/// Lecture transversale (tous types de comptes) pour la connexion et la vérification de permission.
/// L'implémentation Infrastructure interroge toutes les tables de comptes.
/// </summary>
public interface IRegistreUtilisateurs
{
    Task<InfosUtilisateur?> TrouverParEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<InfosUtilisateur?> TrouverParIdAsync(Guid id, CancellationToken cancellationToken = default);
}
