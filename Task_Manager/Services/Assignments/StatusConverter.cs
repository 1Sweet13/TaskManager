using Task_Manager.Data;
using Task_Manager.Enums;

namespace Task_Manager.Services.Assignments
{
    public static  class StatusConverter
    {
        public static AssignmentStatus? GetStatus(string status)
        {
            var result = Enum.TryParse(status, ignoreCase: true, out AssignmentStatus parsedRole);

            if(result)
            {
                return parsedRole;
            }
            {
                return null;
            }
        }

    }
}
