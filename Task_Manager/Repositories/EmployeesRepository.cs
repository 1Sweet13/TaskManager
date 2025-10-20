using Microsoft.EntityFrameworkCore;
using Task_Manager.CustomExceptions;
using Task_Manager.Data;
using Task_Manager.Models.Emplouee;
using Task_Manager.Models.Employee;
using Task_Manager.Services.Employees;

namespace Task_Manager.Repositories
{
    public class EmployeesRepository(AssignmentManagerDBContext context)
    {

        public AssignmentManagerDBContext context { get; set; } = context;

        // Получение всех сотрудников
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await context.Employees.ToListAsync();
        }

        // Получение сотрудника по Id
        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null) throw new NotFoundException($"Сотрудник с данным Id - {employeeId} не найден");

            return employee;
        }

        // Создание нового сотрудника
        public async Task<Employee> CreateNewEmployeeAsync(CreateRequestEmployee createEmployee)
        {
                if (createEmployee == null)
                    throw new ArgumentNullException(nameof(createEmployee), "Employee data is required.");

                var existUserName = await context.Employees.Where(e => e.UserName == createEmployee.user_name).FirstOrDefaultAsync();

                if (existUserName != null) throw new ValidationException($"Employee with username '{createEmployee.user_name}' already exists.");

                EmployeeValidator.Validate(createEmployee);

                Employee newEmployee = EmployeeMapper.ToEmployee(createEmployee);

                await context.Employees.AddAsync(newEmployee);

                await context.SaveChangesAsync();

                return newEmployee;
        }

        // Изменение данных сотрудника
        public async Task ChangeEmployeeData(int employeeId, ChangeRequestEmployee updateEmployee)
        {

            if (updateEmployee == null)
                throw new ArgumentNullException(nameof(updateEmployee), "Employee data is required.");

            Employee? change_employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

            if (change_employee == null) throw new NotFoundException("Сотрудник не найден");

            var existUserName = await  context.Employees.Where(e => e.UserName == updateEmployee.user_name).FirstOrDefaultAsync();

            if (existUserName != null) throw new ValidationException($"Employee with username '{updateEmployee.user_name}' already exists.");

            EmployeeValidator.Validate(updateEmployee);

            change_employee.Name = updateEmployee.name;
            change_employee.Surname = updateEmployee.surname;
            change_employee.Patronymic = updateEmployee.patronymic;
            change_employee.UserName = updateEmployee.user_name;
            change_employee.Role = updateEmployee.role;

           await context.SaveChangesAsync();
            
        }

        // Удаление сотрудника
        public async Task DeleteEmployeeAsync(int employeeId)
        {



            Employee? employee = await context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null) throw new NotFoundException($"Employee with ID - {employeeId} not found");

            var projects = await context.Projects
                    .Where(p => p.ManagerId == employeeId || p.SupervisorId == employeeId)
                    .ToListAsync();

            var assignmentsAsObserver = await context.Assignments
                         .Include(a => a.Observers)
                         .Where(a => a.Observers.Any(o => o.Id == employeeId))
                         .ToListAsync();

            var assignmentsAsPerformer = await context.Assignments
                        .Include(a => a.Performers)
                        .Where(a => a.Performers.Any(p => p.Id == employeeId))
                        .ToListAsync();

            foreach (var p in projects)
            {
                if (p.ManagerId == employeeId) p.ManagerId = null;
                if (p.SupervisorId == employeeId) p.SupervisorId = null;
            }

            foreach (var a in assignmentsAsObserver)
            {
                a.Observers.RemoveAll(o => o.Id == employeeId);
            }

            foreach (var a in assignmentsAsPerformer)
            {
                a.Performers.RemoveAll(p => p.Id == employeeId);
            }

      

            context.Employees.Remove(employee);

            await context.SaveChangesAsync();

     


        }

    }
}
