namespace FileBlog.Features.ExceptionHandling
{
    public class NoPostFoundException : CustomException
    {
        public NoPostFoundException() : base("No post found.", 404) { }
        public NoPostFoundException(string message) : base(message, 404) { }
        public NoPostFoundException(string message, Exception inner) : base(message, inner, 404) { }
    }
}