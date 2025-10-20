using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_Manager.Data;

namespace Task_Manager.Configuration
{
        public class AssignmentConfiguration : IEntityTypeConfiguration<Assignments>
        {
            public void Configure(EntityTypeBuilder<Assignments> builder)
            {

                builder
                   .HasIndex(e => e.Name)
                   .IsUnique();
                

                

                builder
                  .HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId);

                builder
                  .HasOne(e => e.Group)
                  .WithMany()
                  .HasForeignKey(e => e.GroupId);

                 builder
                   .HasMany(t => t.Performers)
                   .WithMany()
                   .UsingEntity("assignment_performers");

                 builder
                   .HasMany(t => t.Observers)
                   .WithMany()
                   .UsingEntity("assignment_observers");
            }
        }
    
}
