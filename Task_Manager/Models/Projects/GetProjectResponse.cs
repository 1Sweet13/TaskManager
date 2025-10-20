namespace Task_Manager.Models.Projects
{
    public class GetProjectResponse
    {
        public int id { get; set; }

        public string name { get; set; }

        public string descripiton { get; set; }

        public int? supervisor_id { get; set; }

        public int? manager_id { get; set; }


    }
}
