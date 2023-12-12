namespace QLKTX.Class.Dtos
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult()
        {
            Success = false;
        }

        public ApiErrorResult(string message)
        {
            Success = false;
            Message = message;
        }

        public ApiErrorResult(string message, int code, T dataObj)
        {
            Success = false;
            Message = message;
            StatusCode = code;
            Data = dataObj;
        }

        public ApiErrorResult(T dataObj)
        {
            Success = false;
            Data = dataObj;
        }
    }
}
