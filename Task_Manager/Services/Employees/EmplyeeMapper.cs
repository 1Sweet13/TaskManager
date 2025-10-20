using Task_Manager.Enums;
using Task_Manager.Models.Emplouee;

namespace Task_Manager.Data
{
    public static class EmployeeMapper
    {
        
        public static Employee ToEmployee(CreateRequestEmployee dto)
        {

            return new Employee
            {
                UserName = dto.user_name,
                Name = dto.name,
                Surname = dto.surname,
                Patronymic = dto.patronymic,
                Role = dto.role,
            };
        }


        public static CreateRequestEmployee ToEmployeeDTO(Employee dto)
        {

            return new CreateRequestEmployee
            {
                user_name = dto.UserName,
                name = dto.Name,
                surname = dto.Surname,
                patronymic = dto.Patronymic,
                role = dto.Role,
            };
        }

    }
}
