using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task_Manager.CustomExceptions;
using Task_Manager.Data;
using Task_Manager.Models.Emplouee;
using Task_Manager.Models.Employee;
using Task_Manager.Repositories;

namespace Task_Manager.Controllers
{
    public class EmployeesController(EmployeesRepository repository) : ControllerBase
    {

        private readonly EmployeesRepository employeesRepository = repository;

        [HttpGet("/api/employees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var result = await employeesRepository.GetAllEmployeesAsync();

                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                return Problem(
                    title: "Произошла ошибка базы данных",
                    detail: ex.Message,
                    statusCode: 400
                    );
            }
            catch (NotFoundException ex)
            {
                return Problem(
                   title: "Ресурс не найден",
                   detail: ex.Message,
                   statusCode: 404
                   );
            }
            catch (Exception ex)
            {

                return Problem(
                   title: "Произошла непредвиденная ошибка",
                   detail: ex.Message,
                   statusCode: 500
                   );
            }
        }


        [HttpGet("/api/employees/{employeeId}")]
        public async Task<IActionResult> GetEmployeesBYId([FromRoute] int employeeId)
        {
            try
            {
                var employee = await employeesRepository.GetEmployeeByIdAsync(employeeId);

                return Ok(employee);

            }
            catch (DbUpdateException ex)
            {
                return Problem(
                    title: "Произошла ошибка базы данных",
                    detail: ex.Message,
                    statusCode: 400
                    );
            }
            catch (NotFoundException ex)
            {
                return Problem(
                   title: "Ресурс не найден",
                   detail: ex.Message,
                   statusCode: 404
                   );
            }
            catch (Exception ex)
            {

                return Problem(
                   title: "Произошла непредвиденная ошибка",
                   detail: ex.Message,
                   statusCode: 500
                   );
            }

        }


        [HttpPost("/api/employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateRequestEmployee employee)
        {
            try
            {
                var result = await employeesRepository.CreateNewEmployeeAsync(employee);

                return Ok("Пользователь успешно создан");
            }
            catch (DbUpdateException ex)
            {
                return Problem(
                    title: "Произошла ошибка базы данных",
                    detail: ex.Message,
                    statusCode: 400
                    );
            }
            catch (ConflictException ex)
            {
                return Problem(
                  title: "Конфликт при добавлении/удалении",
                  detail: ex.Message,
                  statusCode: 400
                  );
            }
            catch (ValidationException ex)
            {
                return Problem(
                   title: "Ошибка валидации",
                   detail: ex.Message,
                   statusCode: 400
                   );
            }
            catch (NotFoundException ex)
            {
                return Problem(
                   title: "Ресурс не найден",
                   detail: ex.Message,
                   statusCode: 404
                   );
            }
            catch (Exception ex)
            {

                return Problem(
                   title: "Произошла непредвиденная ошибка",
                   detail: ex.Message,
                   statusCode: 500
                   );
            }

        }


        [HttpPut("/api/employees/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId,[FromBody] ChangeRequestEmployee data)
        {
            try
            {
                await employeesRepository.ChangeEmployeeData(employeeId,data);

                return Ok("Данные пользователя успешно обновлены");

            }
            catch (DbUpdateException ex)
            {
                return Problem(
                    title: "Произошла ошибка базы данных",
                    detail: ex.Message,
                    statusCode: 400
                    );
            }
            catch (ConflictException ex)
            {
                return Problem(
                  title: "Конфликт при добавлении/удалении",
                  detail: ex.Message,
                  statusCode: 400
                  );
            }
            catch (ValidationException ex)
            {
                return Problem(
                   title: "Ошибка валидации",
                   detail: ex.Message,
                   statusCode: 400
                   );
            }
            catch (NotFoundException ex)
            {
                return Problem(
                   title: "Ресурс не найден",
                   detail: ex.Message,
                   statusCode: 404
                   );
            }
            catch (Exception ex)
            {

                return Problem(
                   title: "Произошла непредвиденная ошибка",
                   detail: ex.Message,
                   statusCode: 500
                   );
            }

        }


        [HttpDelete("/api/employees/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int employeeId)
        {
            try
            {
                await employeesRepository.DeleteEmployeeAsync(employeeId);

                return Ok("Пользователь успешно удален");
            }
            catch (DbUpdateException ex)
            {
                return Problem(
                    title: "Произошла ошибка базы данных",
                    detail: ex.Message,
                    statusCode: 400
                    );
            }
            catch (ConflictException ex)
            {
                return Problem(
                  title: "Конфликт при добавлении/удалении",
                  detail: ex.Message,
                  statusCode: 400
                  );
            }
            catch (ValidationException ex)
            {
                return Problem(
                   title: "Ошибка валидации",
                   detail: ex.Message,
                   statusCode: 400
                   );
            }
            catch (NotFoundException ex)
            {
                return Problem(
                   title: "Ресурс не найден",
                   detail: ex.Message,
                   statusCode: 404
                   );
            }
            catch (Exception ex)
            {

                return Problem(
                   title: "Произошла непредвиденная ошибка",
                   detail: ex.Message,
                   statusCode: 500
                   );
            }
        }



    }
}
