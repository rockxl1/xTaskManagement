using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.Domain.Entities;

namespace xTask.Infrastructure.Data
{
    public class xTaskManagementContext: DbContext
    {
        public xTaskManagementContext(DbContextOptions<xTaskManagementContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; 
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        private string _connectionString;
        public xTaskManagementContext(string connectionString)
        {
            _connectionString = connectionString;

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; 
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add indexes for performance optimization
            modelBuilder.Entity<Todo>()
                .HasIndex(e => e.CreatedBy)
                .HasDatabaseName("IX_Todo_CreatedBy");

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasIndex(e => e.CreatedBy)
                .HasDatabaseName("IX_Task_CreatedBy");

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasIndex(e => e.TodoID)
                .HasDatabaseName("IX_Task_TodoID");

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasIndex(e => new { e.CreatedBy, e.TodoID })
                .HasDatabaseName("IX_Task_CreatedBy_TodoID");

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasIndex(e => new { e.TodoID, e.Order })
                .HasDatabaseName("IX_Task_TodoID_Order");
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
       
    }
}
