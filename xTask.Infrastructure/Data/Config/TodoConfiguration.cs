using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.Domain.Entities;
using xTask.Infrastructure.Contants;

namespace xTask.Infrastructure.Data.Config
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable(TablesNamesConstants.TODOs);
           
            builder.HasIndex(x => x.Name);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);


            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(254);
            builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(254);
        }
    }
}
