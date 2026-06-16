using CatalogueEmploi.Domaine.Entites;
using CatalogueEmploi.Domaine.ObjetsValeur;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogueEmploi.Infrastructure.Persistance.Configurations;

public sealed class AnnonceEmploiConfiguration : IEntityTypeConfiguration<AnnonceEmploi>
{
    public void Configure(EntityTypeBuilder<AnnonceEmploi> builder)
    {
        builder.ToTable("AnnoncesEmploi");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.EntrepriseId).IsRequired();

        builder.Property(a => a.Titre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(a => a.TypeContrat)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(a => a.DomaineMetier, dm =>
        {
            dm.Property(d => d.Valeur)
                .HasColumnName("DomaineMetier")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.Property(a => a.DatePublication).IsRequired();
        builder.Property(a => a.EstActive).IsRequired();
    }
}
