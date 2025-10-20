using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_Manager.Data;

namespace Task_Manager.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Projects>
    {
        public void Configure(EntityTypeBuilder<Projects> builder)
        {
            builder.HasIndex(e => e.Name).IsUnique();

            builder.HasOne(p => p.Manager).WithMany().HasForeignKey(p => p.ManagerId);

            builder.HasOne(p => p.Supervisor).WithMany().HasForeignKey(p => p.SupervisorId);
           
        }
    }
}
