using System.ComponentModel.DataAnnotations;
using Task_Manager.Data;
using Task_Manager.Enums;
using Task_Manager.Models.Emplouee;
using Task_Manager.Models.Employee;

namespace Task_Manager.Services.Employees
{
    public static class EmployeeValidator
    {

        public static void Validate(CreateRequestEmployee employee)
        {

            if (string.IsNullOrEmpty(employee.name)) throw new ValidationException("Name is required");

            if (string.IsNullOrEmpty(employee.surname)) throw new ValidationException("Surname is required");

            if (string.IsNullOrEmpty(employee.patronymic)) throw new ValidationException("Patronymic is required");

            if (string.IsNullOrEmpty(employee.user_name)) throw new ValidationException("UserName is required");

            if (string.IsNullOrEmpty(employee.role)) throw new ValidationException("Role is required");


            if (!Enum.TryParse(employee.role, ignoreCase: true, out Role parsedRole))
            {
                throw new ValidationException("Invalid Role");
            }

        }


        public static void Validate(ChangeRequestEmployee employee)
        {

            if (string.IsNullOrEmpty(employee.name)) throw new ValidationException("Name is required");

            if (string.IsNullOrEmpty(employee.surname)) throw new ValidationException("Surname is required");

            if (string.IsNullOrEmpty(employee.patronymic)) throw new ValidationException("Patronymic is required");

            if (string.IsNullOrEmpty(employee.user_name)) throw new ValidationException("UserName is required");

            if (string.IsNullOrEmpty(employee.role)) throw new ValidationException("Role is required");


            if (!Enum.TryParse(employee.role, ignoreCase: true, out Role parsedRole))
            {
                throw new ValidationException("Invalid Role");
            }

        }




        public static void Validate(Employee employee)
        {
           CreateRequestEmployee employeeDTO = EmployeeMapper.ToEmployeeDTO(employee);

           Validate(employeeDTO);

        }



    }
}
