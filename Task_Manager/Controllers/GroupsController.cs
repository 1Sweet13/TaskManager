using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Task_Manager.CustomExceptions;
using Task_Manager.Data;
using Task_Manager.Repositories;
using ValidationException = Task_Manager.CustomExceptions.ValidationException;

namespace Task_Manager.Controllers
{
    public class GroupsController(GroupRepository groupRepository) : ControllerBase
    {

        private readonly GroupRepository groupRepository = groupRepository;

        [HttpGet("api/groups")]
        public async Task<IActionResult> GetAllGroups()
        {
            try
            {
               var groups  =  await groupRepository.GetAllGroupsAsync();

                return Ok(groups);
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


        [HttpGet("api/groups/{groupId}")]
        public async Task<IActionResult> GetGroupByID(int groupId)
        {
            try
            {
                var result = await groupRepository.GetGroupByIdAsync(groupId);

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


        [HttpPost("/api/groups")]
        public async Task<IActionResult> CreateGroup([FromQuery]string nameGroup)
        {
            try
            {
                await groupRepository.CreateGroupAsync(nameGroup);

                return Ok($"Группа {nameGroup} успешно создана");
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


        [HttpPut("/api/groups/{groupId}")]
        public async Task<IActionResult> UpdateGroup([Required]int groupId, [FromQuery][Required] string newName)
        {
            try
            {
                await groupRepository.ChangeGroupNameAsync(groupId, newName);

                return Ok("Название группы изменено");

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


        [HttpDelete("/api/groups/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            try
            {
                await groupRepository.DeleteGroupAsync(groupId);

                return Ok("Группа успешно удалена");
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
