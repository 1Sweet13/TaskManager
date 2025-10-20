using System.ComponentModel.DataAnnotations;
using Task_Manager.Enums;

namespace Task_Manager.Data
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string UserName { get; set; }

        public string  Role { get; set; }

    }


}
