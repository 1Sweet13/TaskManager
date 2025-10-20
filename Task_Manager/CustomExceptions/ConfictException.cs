namespace Task_Manager.CustomExceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string? message) : base(message) { }
    }

}
