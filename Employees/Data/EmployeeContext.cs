

using Employees.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.Data
{

    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>().HasIndex(a => a.Name).IsUnique();
            modelBuilder.Entity<Department>().HasIndex(a => a.Name).IsUnique();


            modelBuilder.Entity<Position>().HasData(
                new { Id = 1, Name = "Младший программист" },
                new { Id = 2, Name = "Старший программист" },
                new { Id = 3, Name = "Руководитель отдела" },
                new { Id = 4, Name = "Менеджер" },
                new { Id = 5, Name = "Специалист по кадрам" },
                new { Id = 6, Name = "Директор" },
                new { Id = 7, Name = "Бухгалтер" },
                new { Id = 8, Name = "Финансовый директор" },
                new { Id = 9, Name = "Специалист по закупке" }
                );

            modelBuilder.Entity<Department>().HasData(
                new { Id = 1, Name = "Продажа" },
                new { Id = 2, Name = "Разработка" },
                new { Id = 3, Name = "Техническое обслуживание" },
                new { Id = 4, Name = "Кадры" },
                new { Id = 5, Name = "Маркетинг" },
                new { Id = 6, Name = "Логистика" },
                new { Id = 7, Name = "Закупки" }
                );
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public object Position { get; internal set; }
    }
}
