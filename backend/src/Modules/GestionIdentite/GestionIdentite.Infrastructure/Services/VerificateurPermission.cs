using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.Enums;
using GestionIdentite.Domaine.ObjetsValeur;
using GestionIdentite.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GestionIdentite.Infrastructure.Services;

public sealed class VerificateurPermission : IVerificateurPermission
{
    private readonly GestionIdentiteDbContext _dbContext;

    public VerificateurPermission(GestionIdentiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> PossedePermissionAsync(
        Guid utilisateurId, Permission permission, CancellationToken cancellationToken = default)
    {
        var role = await ObtenirRoleAsync(utilisateurId, cancellationToken);
        return role.HasValue && MatricePermissions.PossedePermission(role.Value, permission);
    }

    private async Task<Role?> ObtenirRoleAsync(Guid id, CancellationToken cancellationToken)
    {
        var roleCandidat = await _dbContext.ComptesCandidats
            .Where(c => c.Id == id)
            .Select(c => (Role?)c.Role)
            .FirstOrDefaultAsync(cancellationToken);

        if (roleCandidat.HasValue) return roleCandidat;

        var roleEntreprise = await _dbContext.ComptesEntreprises
            .Where(c => c.Id == id)
            .Select(c => (Role?)c.Role)
            .FirstOrDefaultAsync(cancellationToken);

        if (roleEntreprise.HasValue) return roleEntreprise;

        var roleAdmin = await _dbContext.ComptesAdministrateurs
            .Where(c => c.Id == id)
            .Select(c => (Role?)c.Role)
            .FirstOrDefaultAsync(cancellationToken);

        return roleAdmin;
    }
}
