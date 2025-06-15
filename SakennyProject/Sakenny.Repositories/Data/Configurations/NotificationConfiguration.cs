using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);
            builder.HasOne(u => u.Auther)
                .WithMany()
                .HasForeignKey(u => u.AutherId)
                .OnDelete(DeleteBehavior.NoAction);
      
            builder.Property(n => n.Content)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(n => n.Date)
                .IsRequired()
                .HasColumnType("datetime2");

        }
    }
}
