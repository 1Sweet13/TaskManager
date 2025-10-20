using Microsoft.EntityFrameworkCore;
using Task_Manager.CustomExceptions;
using Task_Manager.Data;
using Task_Manager.Enums;
using Task_Manager.Models.Projects;
using Task_Manager.Services.Projects;

namespace Task_Manager.Repositories
{
    public class ProjectsRepository(AssignmentManagerDBContext context)
    {
        private readonly AssignmentManagerDBContext context = context;

        // Получает все проекты 
        public async Task<List<GetProjectResponse>> GetProjectsAsync()
        {
            return await context.Projects.Select(project => new GetProjectResponse()
            {
                id = project.Id,
                name = project.Name,
                descripiton = project.Description,
                manager_id = project.ManagerId,
                supervisor_id = project.SupervisorId,
            }).ToListAsync();
        }

        // Получает данные о проекте по айди
        public async Task<GetProjectResponse> GetProjectAByIdAsync(int projectId)
        {
            var project = await context.Projects
                .FirstOrDefaultAsync(e => e.Id == projectId);

            if (project == null) throw new NotFoundException($"Project with ID - {projectId} not found"); 

            GetProjectResponse dtoProject = new() { 
                id = project.Id, 
                name = project.Name, 
                descripiton = project.Description,
                manager_id = project.ManagerId,
                supervisor_id = project.SupervisorId,
                
            };


            return dtoProject;
        }

        // Создание нового проекта
        public async Task CreateProjectAsync(CreateRequestProject newProject)
        {

            List<int> missedUserIds = new List<int>();  


            var checkProject = await context.Projects
                .FirstOrDefaultAsync(e => e.Name == newProject.name);

            if (checkProject != null) throw new ConflictException("Проект с таким именем уже существует");

            var checkManager = await context.Employees
                .Where(employee => newProject.manager_id == employee.Id)
                .FirstOrDefaultAsync();

            var checkSupervisor = await context.Employees
                .Where(employee => newProject.supervisor_id == employee.Id)
                .FirstOrDefaultAsync();

            if (checkManager is null && newProject.manager_id != null)
            {
                throw new NotFoundException("Id пользователя на роль менеджера не найдено");
            }
            if (checkSupervisor is  null && newProject.supervisor_id != null)
            {
                throw new NotFoundException("Id пользователя на роль руководителя не найдено");
            }

            if (newProject.manager_id != null && newProject.supervisor_id != null)
            {
                if (newProject.manager_id == newProject.supervisor_id)
                {
                    throw new ConflictException($"Сотрудник не может быть руководителем и менеджером");
                }
            }






            Projects project = new Projects() { 
                Name = newProject.name, 
                Supervisor = checkSupervisor, 
                Manager = checkManager,
                Description = newProject.descripiton };


            await context.Projects.AddAsync(project);


            await context.SaveChangesAsync();
        }

        // Обновление данных проекта
        public async Task UpdateProjectDataAsync(int projectId,UpdateProjectRequest updateProject)
        {

            var project = await context.Projects.Include(e => e.Manager).FirstOrDefaultAsync(e => e.Id == projectId);

            if (project == null) throw new NotFoundException($"Проект с данным айди {projectId} не найден");

            if (string.IsNullOrEmpty(updateProject.name)) throw new ArgumentNullException("Название проекта не может быть пустым");



            if (updateProject.supervisor_id != null)
            {

                var checkSupervisior = await context.Employees.FirstOrDefaultAsync(e => e.Id == updateProject.supervisor_id);
                if (checkSupervisior is null) throw new NotFoundException("Руководитель не найден");

            }

            if(updateProject.manager_id != null)
            {
                var checkManager = await context.Employees.FirstOrDefaultAsync(e => e.Id == updateProject.manager_id);

                if (checkManager is null) throw new NotFoundException("Менеджер не найден");

            }

            if(updateProject.manager_id != null && updateProject.supervisor_id != null)
            {
                if (updateProject.manager_id == updateProject.supervisor_id)
                {
                    throw new ConflictException("Сотрудник не может быть руководителем и менеджером");
                }
            }

            
            project.Name = updateProject.name;
            project.Description = updateProject.description;
            project.ManagerId = updateProject.manager_id;
            project.SupervisorId = updateProject.supervisor_id;


            context.Update(project);

            await context.SaveChangesAsync();


        }

        // Удаление проекта по айди
        public async Task DeleteProjectByIdAsync(int projectId)
        {

            var checkProject = await context.Projects
                .FirstOrDefaultAsync(e => e.Id == projectId);

            if (checkProject == null) throw new NotFoundException("Проект не найден");

            var checkAssigns = await context.Assignments
                .Include(e => e.Project).Where(E => E.ProjectId == projectId).ToListAsync();

            if(checkAssigns.Count == 0)
            {
                context.Remove(checkProject);
            }

            if(checkAssigns.All(e => e.Status == nameof(AssignmentStatus.Canceled)))
            {
                context.RemoveRange(checkAssigns);
                context.Remove(checkProject);         
            }
            else
            {
                throw new Exception("Не удалось удалить проект: Все задачи должны быть в статусе Canceled");
            }


            await context.SaveChangesAsync();

        }



    }
}
