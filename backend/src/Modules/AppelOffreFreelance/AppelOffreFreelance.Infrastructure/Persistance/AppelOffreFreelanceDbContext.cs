using AppelOffreFreelance.Domaine.Entites;
using Microsoft.EntityFrameworkCore;

namespace AppelOffreFreelance.Infrastructure.Persistance;

public sealed class AppelOffreFreelanceDbContext : DbContext
{
    public DbSet<AppelOffre> AppelsOffre => Set<AppelOffre>();
    public DbSet<PropositionFreelance> Propositions => Set<PropositionFreelance>();

    public AppelOffreFreelanceDbContext(DbContextOptions<AppelOffreFreelanceDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppelOffreFreelanceDbContext).Assembly);
    }
}
