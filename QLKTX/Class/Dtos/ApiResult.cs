namespace QLKTX.Class.Dtos
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }
        public int StatusCode { get; set; }

        public T Data { get; set; }
    }
}
