using CatalogueEmploi.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogueEmploi.Infrastructure.Persistance.Configurations;

public sealed class CurriculumVitaeConfiguration : IEntityTypeConfiguration<CurriculumVitae>
{
    public void Configure(EntityTypeBuilder<CurriculumVitae> builder)
    {
        builder.ToTable("CurriculumsVitae");

        builder.HasKey(cv => cv.Id);

        builder.Property(cv => cv.CandidatId).IsRequired();

        builder.Property(cv => cv.Titre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cv => cv.Resume)
            .HasMaxLength(5000);

        builder.Property(cv => cv.DateCreation).IsRequired();
        builder.Property(cv => cv.DateMiseAJour).IsRequired();

        // Stockage des compétences en JSON
        builder.Property(cv => cv.CompetencesPrincipales)
            .HasConversion(
                v => string.Join(";", v),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList().AsReadOnly());

        builder.HasIndex(cv => cv.CandidatId).IsUnique();
    }
}
