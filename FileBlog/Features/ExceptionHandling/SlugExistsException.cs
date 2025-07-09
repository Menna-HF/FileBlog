namespace FileBlog.Features.ExceptionHandling
{
    public class SlugExistsException : CustomException
    {
        public SlugExistsException() : base("Slug already exists.", 409) { }
        public SlugExistsException(string message) : base(message, 409) { }
        public SlugExistsException(string message, Exception inner) : base(message, inner, 409) { }
    }
}