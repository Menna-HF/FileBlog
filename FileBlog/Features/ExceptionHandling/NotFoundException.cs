namespace FileBlog.Features.ExceptionHandling
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message, 404) { }
        public NotFoundException(string message, Exception inner) : base(message, inner, 404) { }
    }
}