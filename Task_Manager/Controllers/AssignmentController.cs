using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Manager.CustomExceptions;
using Task_Manager.Models.Assignment;
using Task_Manager.Models.Tasks;
using Task_Manager.Repositories;
using ValidationException = Task_Manager.CustomExceptions.ValidationException;

namespace Task_Manager.Controllers
{
    public class AssignmentController(AssignmentRepository tasksRepository) : ControllerBase
    {

        private readonly AssignmentRepository tasksRepository = tasksRepository;

        [HttpGet("/api/assignments")]
        public async Task<IActionResult> GetTasks([FromQuery] int? employeeId, [FromQuery] int? taskGroupId, [FromQuery] int? projectId)
        {
            try
            {
                var result = await tasksRepository
                    .GeAssignmentsAsync(employeeId, taskGroupId, projectId);

                return Ok(result);
            }
            catch(DbUpdateException ex)
            {
                return Problem(
                    title: "Произошла ошибка базы данных",
                    detail: ex.Message,
                    statusCode: 400
                    );
            }
            catch(NotFoundException ex)
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

        [HttpGet("/api/assignments/{assignId}")]
        public async Task<IActionResult> GetAssignmentByID([FromRoute]int assignId)
        {
            try
            {
                var task = await tasksRepository.GetAssignmentByIdAsync(assignId);

                return Ok(task);
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

        [HttpPost("/api/assignments")]
        public async Task<IActionResult> CreateAssignment([FromBody]CreateRequestAssignment newAssign)
        {
            try
            {
                await tasksRepository.CreateAssignmentAsync(newAssign);

                return Ok(newAssign);
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
        
        [HttpPut("/api/assignments")]
        public async Task<IActionResult> ChangeAssignment([FromBody] ChangeRequestAssigment updateAssign)
        {
            try
            {
                await tasksRepository.ChangeAssignmentAsync(updateAssign);

                return Ok("Задача успешно обновлена");
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


        [HttpPost("/api/assignments/{assignId}/observers/{employeeId}")]
        public async Task<IActionResult> AddObserver([FromRoute] int assignId, [FromRoute] int employeeId)
        {
            try
            {
                await tasksRepository.AddObserverForAssignmentAsync(assignId, employeeId);


                return Ok("Наблюдатель успешно добавлен");
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


        [HttpPost("/api/assignments/{assignId}/performers/{employeeId}")]
        public async Task<IActionResult> AddPerformer([FromRoute] int assignId, [FromRoute] int employeeId)
        {
            try
            {
                await tasksRepository.AddPerformerForAssignmentAsync(assignId, employeeId);


                return Ok("Исполнитель успешно добавлен");
            }
            catch (DbUpdateException ex)
            {
                return Problem(
                    title: "Произошла ошибка базы данных",
                    detail: ex.Message,
                    statusCode: 400
                    );
            }
            catch(ConflictException ex)
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


        [HttpDelete("/api/assignments/{assignId}/observers/{employeeId}")]
        public async Task<IActionResult> DeleteObserver([FromRoute] int assignId, [FromRoute] int employeeId)
        {
            try
            {
                await tasksRepository.DeleteObserverForAssignmentAsync(assignId, employeeId);


                return Ok("Наблюдатель успешно удален");
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


        [HttpDelete("/api/assignments/{assignId}/performers/{employeeId}")]
        public async Task<IActionResult> DeletePerformer([FromRoute] int assignId, [FromRoute] int employeeId)
        {
            try
            {
                await tasksRepository.DeletePerformerForAssignmentAsync(assignId, employeeId);


                return Ok("Исполнитель успешно удален");
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


        [HttpPatch("/api/assignments/{assignId}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int assignId, string newStatus)
        {
            try
            {
                await tasksRepository.ChangeAssignmentStatusAsync(assignId, newStatus);

                return Ok("Статус задачи успешно изменен");
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


        [HttpDelete("/api/assignments/{assignId}")]
        public async Task<IActionResult> DeleteAssignment([FromRoute] int assignId)
        {
            try
            {
                await tasksRepository.DeleteAssignmentAsync(assignId);


                return Ok("Задача успешно удалена");
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
