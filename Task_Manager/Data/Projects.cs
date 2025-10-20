using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Manager.Data
{
    public class Projects
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string Description { get; set; } 

        public int? SupervisorId { get; set; }

        public Employee Supervisor { get; set; }

        public int? ManagerId { get; set; }

        public Employee Manager { get; set; }

    }
}
