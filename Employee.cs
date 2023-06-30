using Microsoft.EntityFrameworkCore;

namespace MinimalAPI
{
    public class Employee
    {
        public Guid Id { get; set; } = new Guid();
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Salary { get; set; }
        public int Experience { get; set; }
        public string? Role { get; set; }
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Employee> Employee => Set<Employee>();
    }
}
