using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;
using System.Reflection.Emit;

namespace Sakenny.Repository.Data.Configurations
{
    public class PostFavouriteConfiguration : IEntityTypeConfiguration<PostFavouriteList>
    {
        public void Configure(EntityTypeBuilder<PostFavouriteList> builder)
        {
            builder
        .HasKey(pf => new { pf.PostId, pf.FavouriteListId });

            builder
                .HasOne(pf => pf.Post)
                .WithMany(p => p.PostFavouriteLists)
                .HasForeignKey(pf => pf.PostId);

            builder
                .HasOne(pf => pf.FavouriteList)
                .WithMany(pf=>pf.FavouriteListPost)
                .HasForeignKey(pf => pf.FavouriteListId);
            builder.HasIndex(p => p.FavouriteListId);
        }
    }
}
