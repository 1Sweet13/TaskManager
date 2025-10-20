namespace Task_Manager.Models.Assignment
{
    public class ChangeRequestAssigment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int project_id { get; set; }
        public int group_id { get; set; }
        public string dead_line_time { get; set; }
        public string priority { get; set; }

    }
}
