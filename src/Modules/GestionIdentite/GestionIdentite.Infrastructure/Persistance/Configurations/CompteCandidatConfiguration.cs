using GestionIdentite.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIdentite.Infrastructure.Persistance.Configurations;

public sealed class CompteCandidatConfiguration : IEntityTypeConfiguration<CompteCandidat>
{
    public void Configure(EntityTypeBuilder<CompteCandidat> builder)
    {
        builder.ToTable("ComptesCandidats");

        builder.HasKey(c => c.Id);

        // Email est un objet valeur : ses données sont stockées dans les colonnes
        // de la table propriétaire (pas de table séparée).
        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Valeur)
                        .HasColumnName("Email")
                        .IsRequired()
                        .HasMaxLength(320);
        });

        builder.Property(c => c.MotDePasseHache).IsRequired().HasMaxLength(500);

        builder.Property(c => c.Role)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(c => c.EstBloque)
               .IsRequired();
    }
}
