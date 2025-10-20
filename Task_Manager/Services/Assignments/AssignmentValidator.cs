using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Task_Manager.CustomExceptions;
using Task_Manager.Data;
using Task_Manager.Enums;
using Task_Manager.Models.Tasks;
using ValidationException = Task_Manager.CustomExceptions.ValidationException;

namespace Task_Manager.Services.Tasks
{
    public class AssignmentValidator
    {

        private static readonly AssignmentStatus[] createdStatus = [AssignmentStatus.Current,AssignmentStatus.Backlog];


        public static void Validate(CreateRequestAssignment assignObj)
        {
            if(assignObj is null) throw new ValidationException("Обьект не может быть null");

            if (string.IsNullOrEmpty(assignObj.name)) throw new ValidationException("Требуется название задачи");

            if (string.IsNullOrEmpty(assignObj.description)) throw new ValidationException("Требуется описание");

            if (!Enum.TryParse(assignObj.priority, ignoreCase: true, out Priority parsedRole)) throw new ValidationException("Невалидный приоритет");

      
        }

        public static void CreateValidate(CreateRequestAssignment assignObj)
        {
            Validate(assignObj);

            if (!Enum.TryParse(assignObj.status, ignoreCase: true, out Enums.AssignmentStatus parsedStatus)) throw new ValidationException("Невалидный статус");

            if (!createdStatus.Contains(parsedStatus)) throw new ValidationException("Задача может быть создана в статусах Current или Backlog");

        }

    }
}
