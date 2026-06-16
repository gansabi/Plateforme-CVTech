using AppelOffreFreelance.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppelOffreFreelance.Infrastructure.Persistance.Configurations;

public sealed class PropositionFreelanceConfiguration : IEntityTypeConfiguration<PropositionFreelance>
{
    public void Configure(EntityTypeBuilder<PropositionFreelance> builder)
    {
        builder.ToTable("PropositionsFreelance");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.CandidatId).IsRequired();
        builder.Property(p => p.AppelOffreId).IsRequired();
        builder.Property(p => p.TarifJournalier).HasColumnType("decimal(10,2)").IsRequired();
        builder.Property(p => p.DureeJours).IsRequired();
        builder.Property(p => p.Methodologie).HasMaxLength(2000);
        builder.Property(p => p.DateSoumission).IsRequired();

        builder.HasIndex(p => new { p.CandidatId, p.AppelOffreId }).IsUnique();
    }
}
