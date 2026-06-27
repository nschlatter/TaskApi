using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

namespace TaskApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).HasMaxLength(1000);
                entity.Property(t => t.Priority).HasConversion<int>();
            });

            // Seed data
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "Set up project", Description = "Initialize the .NET REST API", IsCompleted = true, Priority = Models.Priority.High, CreatedAt = DateTime.UtcNow },
                new TaskItem { Id = 2, Title = "Write unit tests", Description = "Cover all service methods", IsCompleted = false, Priority = Models.Priority.Medium, CreatedAt = DateTime.UtcNow },
                new TaskItem { Id = 3, Title = "Deploy to production", Description = "Set up CI/CD pipeline", IsCompleted = false, Priority = Models.Priority.High, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(7) }
            );
        }
    }
}
