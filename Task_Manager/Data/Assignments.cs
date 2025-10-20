using System;
using System.Text.RegularExpressions;

namespace Task_Manager.Data
    {
        public class Assignments
        {
            public int Id { get; set; } 

            public string Name { get; set; }

            public string Description { get; set; }

            public int ProjectId { get; set; }

            public int GroupId { get; set; }

            public Projects Project { get; set; }
            public Groups Group { get; set; } 

            public string Priority { get; set; }

            public DateTime DeadlineTime { get; set; }

            public DateTime DateTimeCreated { get; set; } = DateTime.Now;

            public string Status { get; set; }

            public List<Employee> Performers { get; set; } = [];

            public List<Employee> Observers { get; set; } = [];


        }
}
