namespace Task_Manager.Models.Assignment
{
    public class GetResponseAssignment
    {            
        public int id { get; set; } 

        public string name { get; set; }

        public string description { get; set; }

        public int? project_id { get; set; }

        public int? group_id { get; set; }

        public DateTime? deadline_time { get; set; }

        public DateTime created_date_time { get; set; }

        public string priority { get; set; }

        public string status { get; set; }

        public List<int> performers_ids { get; set; } = [];

        public List<int> observers_ids { get; set; } = [];


    }
}
