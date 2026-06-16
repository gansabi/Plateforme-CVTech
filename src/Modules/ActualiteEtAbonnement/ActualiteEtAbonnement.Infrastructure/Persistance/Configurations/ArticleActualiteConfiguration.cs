using ActualiteEtAbonnement.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActualiteEtAbonnement.Infrastructure.Persistance.Configurations;

public sealed class ArticleActualiteConfiguration : IEntityTypeConfiguration<ArticleActualite>
{
    public void Configure(EntityTypeBuilder<ArticleActualite> builder)
    {
        builder.ToTable("ArticlesActualite");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.AuteurId).IsRequired();
        builder.Property(a => a.Titre).IsRequired().HasMaxLength(300);
        builder.Property(a => a.Contenu).IsRequired().HasMaxLength(10000);
        builder.Property(a => a.DatePublication).IsRequired();

        builder.OwnsOne(a => a.DomaineMetier, dm =>
        {
            dm.Property(d => d.Valeur).HasColumnName("DomaineMetier").IsRequired().HasMaxLength(100);
        });
    }
}
