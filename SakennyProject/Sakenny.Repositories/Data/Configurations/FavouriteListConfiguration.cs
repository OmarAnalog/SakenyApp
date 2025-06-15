
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;

namespace Sakenny.Repository.Data.Configurations
{
    internal class FavouriteListConfiguration : IEntityTypeConfiguration<FavouriteList>
    {
        public void Configure(EntityTypeBuilder<FavouriteList> builder)
        {
            builder.HasKey(fl => fl.Id);

            // One-to-One Relationship with User
            builder.HasOne(fl => fl.User)
                .WithOne(u => u.FavouriteList) // Ensure it is one-to-one
                .HasForeignKey<FavouriteList>(fl => fl.UserId) // Define foreign key explicitly
                .OnDelete(DeleteBehavior.Cascade); // Delete favourite list when user is deleted
        }
    }
}
