namespace Task_Manager.Models.Projects
{
    public class UpdateProjectRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public int? supervisor_id { get; set; }
        public int? manager_id { get; set; }
    }
}
