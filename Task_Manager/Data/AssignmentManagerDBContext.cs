using Microsoft.EntityFrameworkCore;
using Task_Manager.Configuration;


namespace Task_Manager.Data
{
    public class AssignmentManagerDBContext : DbContext
    {

        public  AssignmentManagerDBContext (DbContextOptions<AssignmentManagerDBContext> options) : base(options) { }

        public DbSet<Assignments> Assignments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Projects> Projects { get; set; }

        public DbSet<Groups> Groups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new GroupsConfiguration());
            modelBuilder.ApplyConfiguration (new ProjectConfiguration());

            base.OnModelCreating(modelBuilder);

        }


    }
}
