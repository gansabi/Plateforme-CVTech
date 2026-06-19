using GestionIdentite.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionIdentite.Infrastructure.Persistance.Configurations;

public sealed class CompteEntrepriseConfiguration : IEntityTypeConfiguration<CompteEntreprise>
{
    public void Configure(EntityTypeBuilder<CompteEntreprise> builder)
    {
        builder.ToTable("ComptesEntreprises");
        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Valeur)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(320);
        });

        builder.Property(c => c.NomEntreprise).IsRequired().HasMaxLength(200);
        builder.Property(c => c.MotDePasseHache).IsRequired().HasMaxLength(500);
        builder.Property(c => c.Role).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(c => c.EstBloque).IsRequired();
    }
}
