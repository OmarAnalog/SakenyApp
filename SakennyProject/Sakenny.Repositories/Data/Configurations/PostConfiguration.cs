using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(p => p.Id)
                .IsDescending(true);
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);
            builder.HasIndex(p => p.ISDeleted);
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(p => p.UpdatedAt)
                .HasColumnType("datetime2");

            // Foreign Key - Post ↔ User
            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts) // Assuming User has a `Posts` collection
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Delete posts if the user is deleted
            // I had problem here that cycle might happen when deleting
            builder.HasOne(p => p.Unit)
                .WithMany()
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many - Post ↔ Comments
            builder.HasMany(p => p.Comments)
                .WithOne(c => c.Post) // Assuming Comment has a `Post` property
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Delete comments when post is deleted

            // One-to-Many - Post ↔ Likes
            builder.HasMany(p => p.Likes)
                .WithOne(l => l.Post) // Assuming Like has a `Post` property
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.NoAction); // Delete likes when post is deleted
        }
    }
}
