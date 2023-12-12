using System.Collections.Generic;

namespace QLKTX.Class.Dtos
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult()
        {
            Success = true;
        }

        public ApiSuccessResult(string message)
        {
            Success = true;
            Message = message;
        }

        public ApiSuccessResult(string message, int code, T dataObj)
        {
            Success = true;
            Message = message;
            StatusCode = code;
            Data = dataObj;
        }

        public ApiSuccessResult(T dataObj)
        {
            Success = true;
            Data = dataObj;
        }
    }
}
