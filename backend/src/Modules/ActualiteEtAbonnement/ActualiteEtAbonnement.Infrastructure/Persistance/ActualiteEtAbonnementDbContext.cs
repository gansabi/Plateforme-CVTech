using ActualiteEtAbonnement.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace ActualiteEtAbonnement.Infrastructure.Persistance;

public sealed class ActualiteEtAbonnementDbContext : DbContext
{
    public DbSet<ArticleActualite> Articles => Set<ArticleActualite>();
    public DbSet<Abonnement> Abonnements => Set<Abonnement>();
    public DbSet<Notification> Notifications => Set<Notification>();

    public ActualiteEtAbonnementDbContext(DbContextOptions<ActualiteEtAbonnementDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActualiteEtAbonnementDbContext).Assembly);
    }
}
