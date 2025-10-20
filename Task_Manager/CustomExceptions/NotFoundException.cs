namespace Task_Manager.CustomExceptions
{

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

}
