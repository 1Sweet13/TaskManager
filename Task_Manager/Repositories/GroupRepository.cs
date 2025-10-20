using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Task_Manager.CustomExceptions;
using Task_Manager.Data;
using Task_Manager.Enums;
using Task_Manager.Models.GroupTasks;

namespace Task_Manager.Repositories
{
    public class GroupRepository(AssignmentManagerDBContext context)
    {

        private readonly AssignmentManagerDBContext context = context;

        // Получение всех групп
        public async Task<List<GetResponseGroup>> GetAllGroupsAsync()
        {
            return await context.Groups.Select(e => new GetResponseGroup { id = e.Id,name =e.Name }).ToListAsync();
        }

        // Получение группы по Id
        public async Task<GetResponseGroup> GetGroupByIdAsync(int groupId)
        {
            var checkGroup = await context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);

            if (checkGroup == null) throw new NotFoundException($"Task group  with ID - {groupId} not found");

            GetResponseGroup group = new();
            group.id = checkGroup.Id;
            group.name = checkGroup.Name;

            return group;
        }

        // Создание новой группы
        public async Task<GetResponseGroup> CreateGroupAsync(string nameGroup)
        {
            var checkGroup = await context.Groups.FirstOrDefaultAsync(e => e.Name == nameGroup);

            GetResponseGroup newGroup = new GetResponseGroup();

            if (checkGroup != null)
            {
                throw new ConflictException("A group with this name already exists");
            }
            else
            {
                checkGroup = new Groups { Name = nameGroup };

                await context.Groups.AddAsync(checkGroup);
                
                newGroup.id = checkGroup.Id;
                newGroup.name = checkGroup.Name;

            }

            await context.SaveChangesAsync();

            return newGroup;
        }

        // Изменение группы 
        public async Task ChangeGroupNameAsync(int groupId, string newName)
        {

            var group = await context.Groups.FirstOrDefaultAsync(e => e.Id == groupId);

            if (group == null) throw new NotFoundException($"Группа с данным айди -{groupId} не найдена");

            if (string.IsNullOrEmpty(newName)) throw new ArgumentNullException("Имя группы не может быть пустым");

            group.Name = newName;

            await context.SaveChangesAsync();
        }

        // Удаление группы по Id
        public async Task DeleteGroupAsync(int groupId)
        {
            var group = await context.Groups
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                throw new NotFoundException("Группа не найдена");

            var tasks = context.Assignments
                .Where(a => a.GroupId == groupId)
                .ToList();

           
            if (tasks.Count > 0 && !tasks.All(a => a.Status == nameof(AssignmentStatus.Canceled)))
            {
                throw new ConflictException("Не удалось удалить группу: все задачи должны быть в статусе 'Canceled'.");
            }

 
            context.RemoveRange(tasks);

            context.Remove(group);

            await context.SaveChangesAsync();

        }


    }
}
