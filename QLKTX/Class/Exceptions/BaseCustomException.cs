using System.Net;

namespace QLKTX.Class.Exceptions
{
    public class BaseCustomException : Exception
    {
        public List<string> ErrorMessages { get; }

        public HttpStatusCode StatusCode { get; }

        public BaseCustomException(string message, List<string> errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            ErrorMessages = errors;
            StatusCode = statusCode;
        }
    }
}
