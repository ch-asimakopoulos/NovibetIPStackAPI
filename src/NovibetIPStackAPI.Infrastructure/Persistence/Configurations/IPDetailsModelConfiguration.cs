using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovibetIPStackAPI.Core.Models.IPRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovibetIPStackAPI.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// The database specific configurations for the IPDetailsModel.
    /// </summary>
    public class IPDetailsModelConfiguration : IEntityTypeConfiguration<IPDetailsModel>
    {
        public void Configure(EntityTypeBuilder<IPDetailsModel> builder)
        {
            builder.Property(det => det.IP)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("IPAddress");

            builder.HasIndex(det => det.IP).IsUnique();
        }
    }
}
