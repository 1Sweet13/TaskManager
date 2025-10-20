using Task_Manager.Data;

namespace Task_Manager.Models.Tasks
{
    public class CreateRequestAssignment
    {
        public string name { get; set; }

        public string description { get; set; }

        public int project_id { get; set; }

        public int group_id { get; set; }

        public string deadline_time { get; set; }

        public string priority { get; set; }

        public string status { get; set; }

        public List<int> performers_ids { get; set; } = [];

        public List<int> observer_ids { get; set; } = [];
    }
}
