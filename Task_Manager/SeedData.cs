using Task_Manager.Data;
using Task_Manager.Enums;

namespace Task_Manager
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scoped = serviceProvider.CreateScope();

            using var context = scoped.ServiceProvider.GetRequiredService<AssignmentManagerDBContext>();

            // Если уже есть данные — не заполняем
            if (context.Groups.Any())
                return;

            // === 1. Группы (20+) ===
            var groups = Enumerable.Range(1, 25)
                .Select(i => new Groups { Name = $"Группа разработки {i}" })
                .ToList();
            context.Groups.AddRange(groups);
            context.SaveChanges();

            // === 2. Сотрудники (30+) с ролями ===
            var roles = Enum.GetValues<Role>().Cast<Role>().ToArray();
            var employees = Enumerable.Range(1, 40)
                .Select(i => new Employee
                {
                    Name = $"Имя{i}",
                    Surname = $"Фамилия{i}",
                    Patronymic = $"Отчество{i}",
                    UserName = $"user{i}",
                    Role = roles[i % roles.Length].ToString()
                })
                .ToList();
            context.Employees.AddRange(employees);
            context.SaveChanges();

            // === 3. Проекты (20+) ===
            var projects = Enumerable.Range(1, 22)
                .Select(i => new Projects
                {
                    Name = $"Проект «Альфа-{i}»",
                    Description = $"Описание проекта {i}. Цель: автоматизация процессов.",
                    ManagerId = employees[(i * 2) % employees.Count].Id,
                    SupervisorId = employees[(i * 3 + 5) % employees.Count].Id
                })
                .ToList();
            context.Projects.AddRange(projects);
            context.SaveChanges();

            // === 4. Задачи (Assignments) — 50+ ===
            var statuses = Enum.GetValues<AssignmentStatus>().Cast<AssignmentStatus>().ToArray();
            var priorities = Enum.GetValues<Priority>().Cast<Priority>().ToArray();

            var assignments = new List<Assignments>();
            for (int i = 1; i <= 50; i++)
            {
                var assignment = new Assignments
                {
                    Name = $"Задача: Реализовать модуль {i}",
                    Description = $"Подробное описание задачи {i}. Требуется интеграция с API.",
                    ProjectId = projects[i % projects.Count].Id,
                    GroupId = groups[i % groups.Count].Id,
                    Priority = priorities[i % priorities.Length].ToString(),
                    DeadlineTime = DateTime.Now.AddDays(7 + i * 2),
                    DateTimeCreated = DateTime.Now.AddDays(-i),
                    Status = statuses[i % statuses.Length].ToString()
                    // Performers и Observers заполним ниже (если поддерживается many-to-many)
                };

                // Выбираем 2–3 исполнителя и 1–2 наблюдателя
                var performerIds = new[] {
                employees[i % employees.Count].Id,
                employees[(i + 7) % employees.Count].Id
            }.Distinct().ToList();

                var observerIds = new[] {
                employees[(i + 15) % employees.Count].Id
            }.Distinct().ToList();

                assignments.Add(assignment);
            }

            context.Assignments.AddRange(assignments);
            context.SaveChanges();


        }

    }
}
