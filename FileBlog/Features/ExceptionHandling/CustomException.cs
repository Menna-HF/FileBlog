namespace FileBlog.Features.ExceptionHandling
{
    public class CustomException : Exception
    {
        public int StatusCode { get; }
        public CustomException() { StatusCode = 400; }
        public CustomException(string infoMessage, int statusCode = 400) : base(infoMessage) { StatusCode = statusCode; }
        public CustomException(string infoMessage, Exception exception, int statusCode = 400) : base(infoMessage, exception) { StatusCode = statusCode; }
    }
}