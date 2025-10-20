using System.ComponentModel.DataAnnotations.Schema;
using Task_Manager.Data;

namespace Task_Manager.Services.Projects
{
    public class CreateRequestProject
    {
        public string name { get; set; }

        public string descripiton { get; set; }

        public int? supervisor_id { get; set; }

        public int? manager_id { get; set; }
    }
}
