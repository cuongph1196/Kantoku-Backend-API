using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using System.Text.RegularExpressions;
using System.Text;
using QLKTX.Class.Entities;
using Newtonsoft.Json;
using System.Web;

namespace QLKTX.Class.Middleware.LogRequest
{
    public class LogRequestMiddleware : IMiddleware
    {
        //private readonly RequestDelegate next;
        private readonly ILogger<LogRequestMiddleware> _logger;
        private readonly string _loggingMethod = "DELETE,POST,PUT";
        private readonly IApiLogRepository _logApiRepository;
        public LogRequestMiddleware(ILogger<LogRequestMiddleware> logger, IApiLogRepository logApiRepository)
        {
            //this.next = next;
            _logger = logger;
            _logApiRepository = logApiRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var url = UriHelper.GetDisplayUrl(context.Request);

            StringBuilder consoleOutputStringBuilder = new StringBuilder();

            consoleOutputStringBuilder.Append($"\n┌=={DateTime.Now.ToString()} =====LogRequestMiddleware start=========================┐");

            consoleOutputStringBuilder.Append($"\n{DateTime.Now.ToString()}-Request url: {url},\nRequest Method: {context.Request.Method},Request Schem: {context.Request.Scheme}, UserAgent: {context.Request.Headers[HeaderNames.UserAgent].ToString()}");

            consoleOutputStringBuilder.Append($"\n|--{DateTime.Now.ToString()}1. ├──-----LogRequestMiddleware Request.Headers start-----|");


            string allkeypair = ""; //ko lưu header
            //IHeaderDictionary headers = context.Request.Headers;
            //foreach (var headerValuePair in headers)
            //{
            //    allkeypair += "\n" + headerValuePair.Key + ":" + headerValuePair.Value;
            //}

            string requestHeadersString = $"Request.Headers:\n {allkeypair.ToString()}";

            consoleOutputStringBuilder.Append($"\n{DateTime.Now.ToString()}Request.Headers:{0}\n" + allkeypair.ToString());
            consoleOutputStringBuilder.Append($"\n|--{DateTime.Now.ToString()} 2.-----LogRequestMiddleware Request.Headers end-----|");
            consoleOutputStringBuilder.Append($"\n|--{DateTime.Now.ToString()} 3.-----LogRequestMiddleware Request.Body start-----|");

            string requestBody = string.Empty;
            var request = context.Request;

            if (!string.IsNullOrEmpty(request.ContentType) && request.ContentType.StartsWith("application/json"))
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 4096, true);
                requestBody = await reader.ReadToEndAsync();

                // rewind for next middleware.
                request.Body.Position = 0;
            }

            consoleOutputStringBuilder.Append($"\n{DateTime.Now.ToString()}-Request Body: {requestBody}");
            consoleOutputStringBuilder.Append($"\n|--{DateTime.Now.ToString()} 4.├──-----LogRequestMiddleware Request.Body end-----|");

            consoleOutputStringBuilder.Append($"\n└=={DateTime.Now.ToString()}   ──=====LogRequestMiddleware end=========================┘");
            string patternBody = @"(login)|(upload-avatar)|(change-password)|(check-password)";
            Match matchBody = Regex.Match(url.ToLower(), patternBody);
            //ko lưu log
            if (!matchBody.Success)
            {
                _logger.LogInformation(consoleOutputStringBuilder.ToString());
            }
            //lưu db log
            if (_loggingMethod.Contains(context.Request.Method.ToString()))
            {
                LoggedInUser loggedInUser = context.Session.GetString(SystemConstants.UserSession) != null
                ? JsonConvert.DeserializeObject<LoggedInUser>(context.Session.GetString(SystemConstants.UserSession)) : null;

                string bodyContent = HttpUtility.UrlDecode(requestBody);
                string transactionId = string.Empty;
                string transactionNo = string.Empty;
                string formId = string.Empty;

                if (bodyContent.Contains("TransactionID"))
                {
                    string patternFindTranID = @"\[TransactionID\]=(\w+)|TransactionID=(\w+)";
                    Match matchTranID = Regex.Match(bodyContent, patternFindTranID);
                    if (matchTranID.Success)
                    {
                        transactionId = !string.IsNullOrEmpty(matchTranID.Groups[1].Value)
                            ? matchTranID.Groups[1].Value
                            : matchTranID.Groups[2].Value;
                    }
                }
                if (bodyContent.Contains("TransactionNo"))
                {
                    string patternFindTranNo = @"\[TransactionNo\]=(\w+)|TransactionNo=(\w+)";
                    Match matchTranNo = Regex.Match(bodyContent, patternFindTranNo);
                    if (matchTranNo.Success)
                    {
                        transactionNo = !string.IsNullOrEmpty(matchTranNo.Groups[1].Value)
                            ? matchTranNo.Groups[1].Value
                            : matchTranNo.Groups[2].Value;
                    }
                }
                if (bodyContent.Contains("ID") || bodyContent.Contains("ID"))
                {
                    string patternFindTranID = @"\[RowKey\]=(\w+)|RowKey=(\w+)|ID=(\w+)";
                    Match matchID = Regex.Match(bodyContent, patternFindTranID);
                    if (matchID.Success)
                    {
                        formId = !string.IsNullOrEmpty(matchID.Groups[1].Value)
                            ? matchID.Groups[1].Value
                            : matchID.Groups[2].Value;
                    }
                }
                //có login mới lưu db log. chứ ko sẽ lưu luôn những api request dư
                if (loggedInUser != null)
                {
                    //nếu login, change pass liên quan mật khẩu thì ko lưu request body
                    if (matchBody.Success)
                    {
                        requestBody = string.Empty;
                    }
                    var resquestLog = new ApiLogVm()
                    {
                        RequestHeaders = allkeypair.ToString(),
                        AbsoluteUri = url,
                        Host = url,
                        RequestBody = requestBody.ToString(),
                        UserHostAddress = ipAddress(context),
                        RequestedMethod = context.Request.Method,
                        StatusCode = string.Empty,
                        RequestType = "Request",
                        AccountLogin = String.IsNullOrEmpty(loggedInUser?.UserID) ? context.User.Identity.Name : loggedInUser?.UserID,
                        TransactionID = transactionId,
                        TransactionNo = transactionNo,
                        FormID = formId
                    };
                    _logApiRepository.Create(resquestLog);
                }
            }
            //end lưu db log
            await next(context);
        }
        private string ipAddress(HttpContext context)
        {
            // get source ip address for the current request
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                return context.Request.Headers["X-Forwarded-For"];
            else
                return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
