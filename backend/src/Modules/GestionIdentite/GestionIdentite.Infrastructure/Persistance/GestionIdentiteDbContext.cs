using GestionIdentite.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace GestionIdentite.Infrastructure.Persistance;

public sealed class GestionIdentiteDbContext : DbContext
{
    public DbSet<CompteCandidat> ComptesCandidats => Set<CompteCandidat>();
    public DbSet<CompteEntreprise> ComptesEntreprises => Set<CompteEntreprise>();
    public DbSet<CompteAdministrateur> ComptesAdministrateurs => Set<CompteAdministrateur>();

    public GestionIdentiteDbContext(DbContextOptions<GestionIdentiteDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestionIdentiteDbContext).Assembly);
    }
}
