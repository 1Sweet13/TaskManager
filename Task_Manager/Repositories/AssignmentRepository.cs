using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Tasks;
using Task_Manager.CustomExceptions;
using Task_Manager.Data;
using Task_Manager.Enums;
using Task_Manager.Models.Assignment;
using Task_Manager.Models.Tasks;
using Task_Manager.Services.Assignments;
using Task_Manager.Services.Tasks;

namespace Task_Manager.Repositories
{
    public class AssignmentRepository(AssignmentManagerDBContext context)
    {

        private readonly AssignmentManagerDBContext context = context;

        private string dateMask = "yyyy-MM-dd HH:mm:ss";

        #region statuses_consistency_steps

        private readonly AssignmentStatus[] startStatuses = [AssignmentStatus.Backlog, AssignmentStatus.Current];

        private readonly AssignmentStatus[] workStatuses = [AssignmentStatus.Active, AssignmentStatus.Testing];

        private readonly AssignmentStatus[] finishStatuses = [AssignmentStatus.Canceled, AssignmentStatus.Completed];

        #endregion


        // Получение всех задач
        public async Task<List<GetResponseAssignment>> GeAssignmentsAsync(int? employeeId, int? groupId, int? projectId) 
        {


                if(employeeId.HasValue)
                {
                    var user = await context.Employees.Where(e => e.Id == employeeId).FirstOrDefaultAsync();
                    if (user == null) throw new NotFoundException("User not found");
                }


                if(groupId.HasValue)
                {
                   var taskGroup = await context.Groups.Where(e => e.Id == groupId).FirstOrDefaultAsync();    
                   if(taskGroup == null) throw new NotFoundException("Group not found");
                }

                if(projectId.HasValue)
                {
                    var project = await context.Projects.Where(e => e.Id == projectId).FirstOrDefaultAsync();
                    if(project == null) throw new NotFoundException("Prject not found");
                }


                var tasks = context.Assignments.AsQueryable();


                if(employeeId.HasValue)
                {
                    var user = await context.Employees.Where(e => e.Id == employeeId).FirstOrDefaultAsync();


                    tasks = tasks.Where(e => e.Performers.Contains(user) || e.Observers.Contains(user));


                }


                if(groupId.HasValue)
                {
                    tasks = tasks.Where(e => e.GroupId == groupId.Value);
                }

                if (projectId.HasValue)
                {
                    tasks = tasks.Where(t => t.ProjectId == projectId.Value);
                }


                var result = tasks.Select(e => new GetResponseAssignment { 
                    id = e.Id,
                    name = e.Name,
                    description = e.Description, 
                    group_id = e.GroupId, 
                    project_id = e.ProjectId,
                    deadline_time = e.DeadlineTime,
                    created_date_time = e.DateTimeCreated,
                    priority = e.Priority,
                    status = e.Status,
                    performers_ids = e.Performers.Select(e => e.Id).ToList(),
                    observers_ids = e.Observers.Select(e => e.Id).ToList(),

                }).ToList();


                return  result;          
        }

        // Получение задачи оп Id
        public async Task<GetResponseAssignment> GetAssignmentByIdAsync(int taskId)
        {
            var task = await context.Assignments.Select( e =>new GetResponseAssignment
            {
                id = e.Id,
                name = e.Name,
                description = e.Description,
                group_id = e.GroupId,
                project_id = e.ProjectId,
                deadline_time = e.DeadlineTime,
                priority = e.Priority,
                status = e.Status,
                performers_ids = e.Performers.Select(e => e.Id).ToList(),
                observers_ids = e.Observers.Select(e => e.Id).ToList()
            }).FirstOrDefaultAsync(e => e.id == taskId);

            if (task == null) throw new NotFoundException($"Задача с данным айди - {taskId} не найдена");

            return task;
        }

        // Создание новой задачи
        public async Task CreateAssignmentAsync(CreateRequestAssignment newAssign)
        {

            AssignmentValidator.CreateValidate(newAssign);

            DateTime deadLineDate = ValidateDate(newAssign.deadline_time);

            if (deadLineDate < DateTime.Now)
            {
                throw new ValidationException("Крайний срок не может быть меньше даты создания");
            }

            var dublicateIds = newAssign.observer_ids.Where(newAssign.performers_ids.Contains).ToList();

            if (dublicateIds.Count != 0) throw new ConflictException($"Пользователи не могут быть наблюдатеями и исполнителями {string.Join(", ", dublicateIds)}");


            var performers = await context.Employees
                        .Where(e => newAssign.performers_ids.Contains(e.Id)).Select(e => e.Id)
                        .ToListAsync();

            var observers = await context.Employees
                        .Where(e => newAssign.observer_ids.Contains(e.Id)).Select(e => e.Id)
                        .ToListAsync();

            var users = newAssign.performers_ids.Concat(newAssign.observer_ids);

            var findUsers = performers.Concat(observers);

            var missingUsers = users.Where(id => !findUsers.Contains(id));

            
            if (missingUsers.Any()) throw new NotFoundException($"Пользователи не найдены: {string.Join(", ", missingUsers)}");

            var checkGroup = await context.Groups.Where(e => e.Id == newAssign.group_id).FirstOrDefaultAsync();
            var checkName = await context.Groups.Where(e => e.Name == newAssign.name).FirstOrDefaultAsync();
            var checkProject = await context.Projects.Where(e => e.Id == newAssign.project_id).FirstOrDefaultAsync();


            if (checkName != null) throw new ConflictException("Группа с таким названием уже существует.");
            if (checkGroup == null) throw new NotFoundException("Группа не найдена");
            if (checkProject == null) throw new NotFoundException("Проект не найден");


            Assignments task = new Assignments();

            task.Name = newAssign.name;
            task.Description = newAssign.description;
            task.GroupId = newAssign.group_id;
            task.ProjectId = newAssign.project_id;
            task.DeadlineTime = deadLineDate;
            task.Priority = newAssign.priority;
            task.Status = newAssign.status;
            task.Performers = await context.Employees.Where(e => newAssign.performers_ids.Contains(e.Id)).ToListAsync();
            task.Observers = await context.Employees.Where(e => newAssign.observer_ids.Contains(e.Id)).ToListAsync();



            await context.Assignments.AddAsync(task);

            await context.SaveChangesAsync();
            


        }

        // Изменение данных задачи
        public async Task ChangeAssignmentAsync(ChangeRequestAssigment updateAssign)
        {
            if (string.IsNullOrEmpty(updateAssign.name)) throw new ValidationException("name не может быть пустой");
            if (string.IsNullOrEmpty(updateAssign.dead_line_time)) throw new ValidationException("dead_line_time не может быть пустым");
            if (string.IsNullOrEmpty(updateAssign.priority)) throw new ValidationException("priority не может быть пустым");


            var assign = await context.Assignments.FirstOrDefaultAsync(a => a.Id == updateAssign.id);

            if (assign == null) throw new NotFoundException("assign не найдена");

            var checkProject = await context.Projects.AnyAsync(e => e.Id == updateAssign.project_id);

            var chekGroup = await context.Groups.AnyAsync(e => e.Id == updateAssign.group_id);

            var checkName = await context.Assignments.AnyAsync(e => e.Name == updateAssign.name);

            if (checkName) throw new ConflictException("name не найден");

            if (!checkProject) throw new NotFoundException("project не найден");

            if (!chekGroup) throw new NotFoundException("group не найдена");
            
     
            DateTime deadLine = ValidateDate(updateAssign.dead_line_time);
            Priority priority = ValidatePriority(updateAssign.priority);

            if(deadLine < assign.DeadlineTime)
            {
                throw new ValidationException("Крайний срок не может быть меньше даты создания");
            }


            assign.Name = updateAssign.name;
            assign.Description = updateAssign.description;
            assign.ProjectId = updateAssign.project_id;
            assign.GroupId = updateAssign.group_id;
            assign.DeadlineTime = deadLine;
            assign.Priority =  updateAssign.priority;
         
            context.Update(assign);

            await context.SaveChangesAsync();
            
        }


        // Добавление наблюдателя для задачи
        public async Task AddObserverForAssignmentAsync(int taskId, int userId)
        {

                var assign = context.Assignments
                .Include(e => e.Observers)
                .Include(e => e.Performers)
                .FirstOrDefault(e => e.Id == taskId);
                
                if (assign is null) throw new NotFoundException("Задача не найдена");

                var user = context.Employees
                    .FirstOrDefault(e => e.Id == userId);

                if (user is null) throw new NotFoundException("Пользователь не найден");


            var checkObs = assign.Observers.Where(e => e.Id == userId).FirstOrDefault();

            if (checkObs != null) throw new ConflictException("Пользователь уже есть в наблюдателях");

            var checkPerf = assign.Performers.Where(e => e.Id == userId).FirstOrDefault();

            if (checkPerf != null) throw new ConflictException("Пользователь не может быть в наблюдателях и исполнителях");


            assign.Observers.Add(user);


            await context.SaveChangesAsync();
            
        }

        // Добавление исполнителя для задачи
        public async Task AddPerformerForAssignmentAsync(int taskId, int userId)
        {

                var task = await context.Assignments
                    .Include(e => e.Observers)
                    .Include(e => e.Performers)
                    .FirstOrDefaultAsync(e => e.Id == taskId);
                if (task is null) throw new NotFoundException("Задача не найдена");

                var user = await context.Employees
                    .FirstOrDefaultAsync(e => e.Id == userId);

                if (user is null) throw new NotFoundException("Пользователь не найден");


                var checkPerf = task.Performers.Where(e => e.Id == userId).FirstOrDefault();

                if (checkPerf != null) throw new ConflictException("Пользователь уже есть в исполнителях");

                var checkObs = task.Observers.Where(e => e.Id == userId).FirstOrDefault();

                if (checkObs != null) throw new ConflictException("Пользователь не может быть в исполнителях и наблюдателях");


                task.Performers.Add(user);


                await context.SaveChangesAsync();            

        }


        // Удаление наблюдателя для задачи
        public async Task DeleteObserverForAssignmentAsync(int taskId, int userId)
        {
            var assign = context.Assignments
                .Include(e => e.Observers)
                .FirstOrDefault(e => e.Id == taskId);

            if (assign is null) throw new NotFoundException("Задача не найдена");

            var user = context.Employees
                .FirstOrDefault(e => e.Id == userId);

            if (user is null) throw new NotFoundException("Пользователь не найден");

            var obs = assign.Observers.Where(e => e.Id == user.Id).FirstOrDefault();

            if (obs is null) throw new NotFoundException("Пользователя нет в наблюдателях");


            assign.Observers.Remove(user);


            await context.SaveChangesAsync();
        }


        // Удаление исполнителя для задачи
        public async Task DeletePerformerForAssignmentAsync(int taskId, int userId)
        {
            var assign = await context.Assignments
                    .Include(e => e.Performers)
                    .FirstOrDefaultAsync(e => e.Id == taskId);

            if (assign is null) throw new NotFoundException("Задача не найдена");


            var user = await context.Employees
                .FirstOrDefaultAsync(e => e.Id == userId);

            if (user is null) throw new NotFoundException("Пользователь не найден");

            var perf = assign.Performers.Where(e => e.Id == userId).FirstOrDefault();

            if (perf is null) throw new NotFoundException("Пользователя нет в исполнителях");
            

            assign.Performers.Remove(user);


            await context.SaveChangesAsync();
        }


        // Изменение статуса задачи
        public async Task ChangeAssignmentStatusAsync(int taskId, string newStatus)
        {
            using var transaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead); 

            var checkTask = await context.Assignments.Where(e => e.Id == taskId).FirstOrDefaultAsync();

            if (checkTask == null) throw new NotFoundException($"Задача не найдена {taskId}");

            
            var taskStatus = StatusConverter.GetStatus(checkTask.Status);

            var inputStatus = StatusConverter.GetStatus(newStatus);

            if (taskStatus == null) throw new ConflictException("Критичиеска ошибка статус у задачи отсутствует");
                 
            if (inputStatus == null) throw new NotFoundException($"Ошибка при изменении статуса, статус не существует - {newStatus}");

            var result = ValidatonBeforeChangeStatus(taskStatus.Value, inputStatus.Value);

            if(result)
            {
                checkTask.Status = inputStatus.Value.ToString();

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return;
            }
            else
            {
                transaction.Rollback();
                throw new ConflictException($"Не удалось обновить статус");     
            }
        
        }

        // Удаление задачи
        public async Task DeleteAssignmentAsync(int taskId)
        {
            var task = await context.Assignments.Where(e => e.Id == taskId).FirstOrDefaultAsync();

            if (task == null) throw
                    new NotFoundException("Задача не найдена");

            AssignmentStatus? status = StatusConverter.GetStatus(task.Status);

            var result = ValidationBeforeDeleteAssign(status.Value);

            if(result)
            {
                context.Remove(task);
                await context.SaveChangesAsync();
                return;
            }

            throw new ConflictException("Ошибка при удалении статус задачи должен быть в конечном или начальных статусах");

        }



        // Валидация статусов



        private bool ValidationBeforeDeleteAssign(AssignmentStatus status)
        {
            if (startStatuses.Contains(status) || finishStatuses.Contains(status))
            {
                return true;
            }
            return false;
        }

        private  bool ValidatonBeforeChangeStatus(AssignmentStatus currentStatus, AssignmentStatus newStatus)
        {
            if (currentStatus == newStatus)
                return true;

            if (newStatus == AssignmentStatus.Canceled)
                return true;

            if (newStatus < currentStatus)
            {
                if (workStatuses.Contains(newStatus) && workStatuses.Contains(currentStatus))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                AssignmentStatus nextStep = currentStatus + 1;

                if (nextStep == newStatus)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        private DateTime ValidateDate(string date)
        {
            if (!DateTime.TryParseExact(date, dateMask, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                throw new ValidationException($"Неверный формат даты: '{date}'. Ожидается: yyyy-MM-dd HH:mm:ss. Пример: 2001-09-08 00:00:00");
            }

            return result;
        }

        // TODO Поправить вывод
        private Priority ValidatePriority(string priority)
        {
            if(!Enum.TryParse(priority, ignoreCase: true, out Priority priorityResult))
            {
                throw new ValidationException($"Неверный приоритет");
            }

            return priorityResult;
        }



    }


}
