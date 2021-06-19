using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xTask.Domain.Entities;
using xTask.Infrastructure.Contants;

namespace xTask.Infrastructure.Data.Config
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable(TablesNamesConstants.Task);

            builder.HasIndex(x => x.Title);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Notes).IsRequired().HasMaxLength(254);


            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(254);
            builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(254);
        }
    }
}
