using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Manager.CustomExceptions;
using Task_Manager.Models.Projects;
using Task_Manager.Repositories;
using Task_Manager.Services.Projects;
using ValidationException = Task_Manager.CustomExceptions.ValidationException;

namespace Task_Manager.Controllers
{
    public class ProjectsController(ProjectsRepository repository) : ControllerBase
    {


        private readonly ProjectsRepository projectsRepository = repository;

        [HttpGet("api/projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var projectList = await projectsRepository.GetProjectsAsync();

                return Ok(projectList);
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

        [HttpGet("api/projects/{projectId}")]
        public async Task<IActionResult> GetProjectsById(int projectId)
        {
            try
            {
                var project = await projectsRepository.GetProjectAByIdAsync(projectId);

                return Ok(project);
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

        [HttpPost("/api/projects")]
        public async Task<IActionResult> CreateProject([FromBody]CreateRequestProject newProject)
        {
            try
            {
                await projectsRepository.CreateProjectAsync(newProject);

                return Ok("Проект успешно создан");
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

        [HttpPut("/api/projects/{projectId}")]
        public async Task<IActionResult> UpdateProjectData(int projectId,[FromBody]UpdateProjectRequest project)
        {
            try
            {
                await projectsRepository.UpdateProjectDataAsync(projectId ,project);

                return Ok("Проект успешно обновлен");

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

        [HttpDelete("/api/projects/{projectId}")]
        public async Task<IActionResult> DeleteProjectByIdAsync(int projectId)
        {
            try
            {
                await projectsRepository.DeleteProjectByIdAsync(projectId);

                return Ok("Проект успешно удален");
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
