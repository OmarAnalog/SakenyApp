using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            // Foreign Key - Comment ↔ User
            builder.HasOne(c => c.User)
                .WithMany(u => u.Comments) // Assuming User has a `Comments` collection
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Delete comments when the user is deleted

            // Foreign Key - Comment ↔ Post
            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments) // Assuming Post has a `Comments` collection
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Delete comments when the post is deleted
        }
    }
}
