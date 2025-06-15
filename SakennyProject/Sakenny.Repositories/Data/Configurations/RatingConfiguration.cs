
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // configure rating Relatinship
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rate)
                .IsRequired()
                .HasColumnType("decimal(3,2)"); // e.g., 4.50

            // Relationships
            builder.HasOne(r => r.RatingUser)
                   .WithMany(u=>u.Ratings)
                   .HasForeignKey(r => r.RatingUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.RatedUser)
                   .WithMany()
                   .HasForeignKey(r => r.RatedUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Unit)
                   .WithMany(u => u.Ratings)
                   .HasForeignKey(r => r.UnitId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new { x.UnitId, x.RatingUserId });
        }
    }
}
