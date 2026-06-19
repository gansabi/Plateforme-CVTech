using ActualiteEtAbonnement.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActualiteEtAbonnement.Infrastructure.Persistance.Configurations;

public sealed class AbonnementConfiguration : IEntityTypeConfiguration<Abonnement>
{
    public void Configure(EntityTypeBuilder<Abonnement> builder)
    {
        builder.ToTable("Abonnements");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.UtilisateurId).IsRequired();
        builder.Property(a => a.DateInscription).IsRequired();

        builder.OwnsOne(a => a.DomaineMetier, dm =>
        {
            dm.Property(d => d.Valeur).HasColumnName("DomaineMetier").IsRequired().HasMaxLength(100);
        });
    }
}
