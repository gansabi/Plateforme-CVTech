using GestionIdentite.Domaine.Enums;

namespace GestionIdentite.Domaine.Contrats;

/// <summary>
/// Contrat public expose par GestionIdentite.
/// Les autres modules l'utilisent pour verifier les permissions avant toute action metier.
/// Cf. .agent/skills/regles-permissions.md
/// </summary>
public interface IVerificateurPermission
{
    Task<bool> PossedePermissionAsync(
        Guid utilisateurId,
        Permission permission,
        CancellationToken cancellationToken = default);
}
