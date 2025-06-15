using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.Configurations
{
    internal class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(l => l.Id);

            // Foreign Key - Like ↔ User
            builder.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign Key - Like ↔ Post
            builder.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(l => l.UserId);
            // Make Sure that User Makes Atmost one like per post
            builder.HasIndex(l => new { l.UserId, l.PostId }).IsUnique();
        }
    }
}
