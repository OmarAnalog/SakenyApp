using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(c => c.Id);

            // User IDs are required
            builder.HasOne(u=>u.FUser)
                .WithMany()
                .HasForeignKey(u => u.FUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(u=>u.SUser)
                .WithMany()
                .HasForeignKey(u => u.SUserId)
                .OnDelete(DeleteBehavior.NoAction);
         
            builder.HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade); // Delete messages when a chat is deleted

            // Ensure that a Chat is unique between two user - Fasting Query
            builder.HasIndex(c => new { c.FUserId, c.SUserId })
                .IsUnique();
        }
    }
}
