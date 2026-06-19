using ActualiteEtAbonnement.Domaine.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActualiteEtAbonnement.Infrastructure.Persistance.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.DestinataireId).IsRequired();
        builder.Property(n => n.Titre).IsRequired().HasMaxLength(300);
        builder.Property(n => n.Message).HasMaxLength(2000);
        builder.Property(n => n.Canal).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(n => n.DateCreation).IsRequired();
        builder.Property(n => n.EstLue).IsRequired();
    }
}
