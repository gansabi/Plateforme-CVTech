using GestionIdentite.Domaine.Contrats;
using GestionIdentite.Domaine.ObjetsValeur;
using Microsoft.EntityFrameworkCore;

namespace GestionIdentite.Infrastructure.Persistance.Repositories;

/// <summary>
/// Lecture transversale : cherche l'utilisateur dans toutes les tables de comptes.
/// Utilisé par ConnecterUtilisateurHandler et VerificateurPermission.
/// </summary>
public sealed class RegistreUtilisateurs : IRegistreUtilisateurs
{
    private readonly GestionIdentiteDbContext _dbContext;

    public RegistreUtilisateurs(GestionIdentiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InfosUtilisateur?> TrouverParEmailAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var emailNormalise = email.ToLowerInvariant();

        var candidat = await _dbContext.ComptesCandidats
            .FirstOrDefaultAsync(c => c.Email.Valeur == emailNormalise, cancellationToken);

        if (candidat is not null)
            return new InfosUtilisateur(candidat.Id, candidat.Email.Valeur,
                candidat.Role, candidat.MotDePasseHache, candidat.EstBloque);

        var entreprise = await _dbContext.ComptesEntreprises
            .FirstOrDefaultAsync(c => c.Email.Valeur == emailNormalise, cancellationToken);

        if (entreprise is not null)
            return new InfosUtilisateur(entreprise.Id, entreprise.Email.Valeur,
                entreprise.Role, entreprise.MotDePasseHache, entreprise.EstBloque);

        var admin = await _dbContext.ComptesAdministrateurs
            .FirstOrDefaultAsync(c => c.Email.Valeur == emailNormalise, cancellationToken);

        if (admin is not null)
            return new InfosUtilisateur(admin.Id, admin.Email.Valeur,
                admin.Role, admin.MotDePasseHache, admin.EstBloque);

        return null;
    }

    public async Task<InfosUtilisateur?> TrouverParIdAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        var candidat = await _dbContext.ComptesCandidats.FindAsync([id], cancellationToken);
        if (candidat is not null)
            return new InfosUtilisateur(candidat.Id, candidat.Email.Valeur,
                candidat.Role, candidat.MotDePasseHache, candidat.EstBloque);

        var entreprise = await _dbContext.ComptesEntreprises.FindAsync([id], cancellationToken);
        if (entreprise is not null)
            return new InfosUtilisateur(entreprise.Id, entreprise.Email.Valeur,
                entreprise.Role, entreprise.MotDePasseHache, entreprise.EstBloque);

        var admin = await _dbContext.ComptesAdministrateurs.FindAsync([id], cancellationToken);
        if (admin is not null)
            return new InfosUtilisateur(admin.Id, admin.Email.Valeur,
                admin.Role, admin.MotDePasseHache, admin.EstBloque);

        return null;
    }
}
