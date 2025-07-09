namespace FileBlog.Features.ExceptionHandling
{
    public class IncorrectInputException : CustomException
    {
        public IncorrectInputException() : base("Incorrect username or password.", 401) { }
        public IncorrectInputException(string message) : base(message, 401) { }
        public IncorrectInputException(string message, Exception inner) : base(message, inner, 401) { }
    }
}