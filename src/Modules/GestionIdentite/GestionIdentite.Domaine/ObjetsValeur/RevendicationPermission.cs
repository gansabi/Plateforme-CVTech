using GestionIdentite.Domaine.Enums;

namespace GestionIdentite.Domaine.ObjetsValeur;

/// <summary>
/// Claim de permission d'un utilisateur authentifié.
/// Représente l'ensemble des permissions actives au moment de la connexion.
/// </summary>
public sealed class RevendicationPermission
{
    public Guid UtilisateurId { get; }
    public Role Role { get; }
    public IReadOnlyCollection<Permission> Permissions { get; }

    public RevendicationPermission(Guid utilisateurId, Role role)
    {
        if (utilisateurId == Guid.Empty)
            throw new ArgumentException("L'identifiant utilisateur ne peut pas être vide.", nameof(utilisateurId));

        UtilisateurId = utilisateurId;
        Role = role;
        Permissions = MatricePermissions.ObtenirPermissions(role);
    }

    public bool PossedePermission(Permission permission) => Permissions.Contains(permission);
}
