using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sakenny.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Repository.Data.Configurations
{
    class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(u=>u.Id);
            builder.HasOne(r => r.From)
            .WithMany()
            .HasForeignKey(r=>r.FromId)
            .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(r=>r.To)
                .WithMany()
                .HasForeignKey(r=>r.ToId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(u => u.Action)
                .HasConversion<string>();
            builder.Property(u => u.ReportTypes)
                .HasConversion<string>();
            builder.Property(u => u.Status)
                .HasConversion<string>();
            builder.Property(u => u.ContentType)
                .HasConversion<string>();
            builder.HasIndex(r => new { r.FromId,r.ContentId});
        }
    }
}
