using System.Text.Json.Serialization;
using Task_Manager.Enums;

namespace Task_Manager.Models.Emplouee
{
    public class CreateRequestEmployee
    {
        public string name { get; set; } 

        public string surname { get; set; } 

        public string patronymic { get; set; }

        public string user_name { get; set; } 

        public string role { get; set; } 


    }
}
