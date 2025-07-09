namespace FileBlog.Features.ExceptionHandling
{
    public class UserExistsException : CustomException
    {
        public UserExistsException() : base("User already exists.", 409) { }
        public UserExistsException(string message) : base(message, 409) { }
        public UserExistsException(string message, Exception inner) : base(message, inner, 409) { }
    }
}