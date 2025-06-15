using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Repository.Data.Configurations
{
    internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(U=>U.Id);
            builder.Property(u => u.Title)
            .IsRequired()
            .HasMaxLength(255);

            builder.Property(u => u.Description)
                .HasMaxLength(1000);

            builder.Property(u => u.Street)
                .HasMaxLength(200);

            builder.Property(u => u.Floor)
                .IsRequired();

            builder.Property(u => u.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(u => u.UnitArea)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(u => u.Rate)
                .IsRequired()
                .HasColumnType("float");

            builder.Property(u => u.IsRented)
                .IsRequired();

            builder.Property(u => u.IsFurnished)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(user => user.Units)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.OwnsOne(u => u.Location, location =>
            {
                location.Property(l => l.Latitude).HasColumnType("decimal(9,6)");
                location.Property(l => l.Longitude).HasColumnType("decimal(9,6)");
            });
            builder.Property(u => u.RentalFrequency)
                .HasConversion<string>();
            builder.Property(u => u.GenderType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(u => u.UnitType)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
