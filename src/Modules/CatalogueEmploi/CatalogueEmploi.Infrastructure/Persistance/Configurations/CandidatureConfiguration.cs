using CatalogueEmploi.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogueEmploi.Infrastructure.Persistance.Configurations;

public sealed class CandidatureConfiguration : IEntityTypeConfiguration<Candidature>
{
    public void Configure(EntityTypeBuilder<Candidature> builder)
    {
        builder.ToTable("Candidatures");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CandidatId).IsRequired();
        builder.Property(c => c.AnnonceId).IsRequired();
        builder.Property(c => c.DatePostulation).IsRequired();
        builder.Property(c => c.LettreMotivation).HasMaxLength(3000);

        builder.HasIndex(c => new { c.CandidatId, c.AnnonceId }).IsUnique();
    }
}
