using System.Net;

namespace QLKTX.Class.Exceptions
{
    public class ForbiddenCustomException : BaseCustomException
    {
        public ForbiddenCustomException(string message) : base(message, null, HttpStatusCode.Forbidden)
        {
        }
    }
}
