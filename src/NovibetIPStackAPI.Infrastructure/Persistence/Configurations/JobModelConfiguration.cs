using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovibetIPStackAPI.Core.Models.BatchRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovibetIPStackAPI.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// The database specific configurations for the JobModel.
    /// </summary>
    public class JobModelConfiguration : IEntityTypeConfiguration<JobModel>
    {
        public void Configure(EntityTypeBuilder<JobModel> builder)
        {
            builder.Property(j => j.JobKey)
                .IsRequired();

            builder.HasIndex(j => j.JobKey).IsUnique();
        }
    }
}
