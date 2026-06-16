using CatalogueEmploi.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace CatalogueEmploi.Infrastructure.Persistance;

public sealed class CatalogueEmploiDbContext : DbContext
{
    public DbSet<AnnonceEmploi> AnnoncesEmploi => Set<AnnonceEmploi>();
    public DbSet<Candidature> Candidatures => Set<Candidature>();
    public DbSet<CurriculumVitae> CurriculumsVitae => Set<CurriculumVitae>();

    public CatalogueEmploiDbContext(DbContextOptions<CatalogueEmploiDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogueEmploiDbContext).Assembly);
    }
}
