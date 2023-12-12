using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace QLKTX.Class.Middleware.LogResponse
{
    public class LogResponseMiddleware : IMiddleware
    {
        private readonly ILogger<LogResponseMiddleware> _logger;
        private readonly string _loggingMethod = "DELETE,POST,PUT";
        private Func<string, Exception, string> _defaultFormatter = (state, exception) => state;

        public LogResponseMiddleware(ILogger<LogResponseMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
            StringBuilder consoleOutputStringBuilder = new StringBuilder();
            consoleOutputStringBuilder.Append($"\n┌==1.{DateTime.Now.ToString()} =====LogResponseMiddleware start=========================┐");
            var url = UriHelper.GetDisplayUrl(context.Request);
            consoleOutputStringBuilder.Append($"\n{DateTime.Now.ToString()} Request url: {url},\nRequest Method: {context.Request.Method},Request Schem: {context.Request.Scheme}, UserAgent: {context.Request.Headers[HeaderNames.UserAgent].ToString()}");

            //Header
            string allkeypair = ""; //ko lưu header
            //IHeaderDictionary headers = context.Response.Headers;
            //foreach (var headerValuePair in headers)
            //{
            //    allkeypair += "\n" + headerValuePair.Key + "：" + headerValuePair.Value;
            //}
            consoleOutputStringBuilder.Append($"\n{DateTime.Now.ToString()}Response.Headers:\n" + allkeypair.ToString());
            consoleOutputStringBuilder.Append($"\n|--{DateTime.Now.ToString()} 2.-----LogResponseMiddleware Response.Headers end-----|");

            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;
            string responseBody;
            newBody.Seek(0, SeekOrigin.Begin);
            responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

            consoleOutputStringBuilder.Append($"\n{DateTime.Now.ToString()} response Body:{responseBody}\n");
            consoleOutputStringBuilder.Append($"\n└=={DateTime.Now.ToString()}   ──=====LogResponseMiddleware end=========================┘");

            if (_loggingMethod.Contains(context.Request.Method.ToString()))
            {
                _logger.LogInformation(consoleOutputStringBuilder.ToString());
            }
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
        }
    }
}
