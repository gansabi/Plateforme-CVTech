using AppelOffreFreelance.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppelOffreFreelance.Infrastructure.Persistance.Configurations;

public sealed class AppelOffreConfiguration : IEntityTypeConfiguration<AppelOffre>
{
    public void Configure(EntityTypeBuilder<AppelOffre> builder)
    {
        builder.ToTable("AppelsOffre");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.EntrepriseId).IsRequired();
        builder.Property(a => a.Titre).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(5000);

        builder.OwnsOne(a => a.DomaineMetier, dm =>
        {
            dm.Property(d => d.Valeur)
                .HasColumnName("DomaineMetier")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.OwnsOne(a => a.BaremeTJM, b =>
        {
            b.Property(x => x.Minimum).HasColumnName("TjmMinimum").HasColumnType("decimal(10,2)");
            b.Property(x => x.Maximum).HasColumnName("TjmMaximum").HasColumnType("decimal(10,2)");
        });

        builder.Property(a => a.DateLimite).IsRequired();
        builder.Property(a => a.DatePublication).IsRequired();
        builder.Property(a => a.Statut).HasConversion<string>().HasMaxLength(50).IsRequired();
    }
}
