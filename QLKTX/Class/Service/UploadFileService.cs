using Microsoft.Extensions.Options;

namespace QLKTX.Class.Service
{
    public interface IUploadFileService
    {
        Task<dynamic> UploadFiles(IFormFile vmFile);
    }
    public class UploadFileService : IUploadFileService
    {
        private readonly IConfiguration _configuration;
        public static IWebHostEnvironment _environment;
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;
        public static readonly string[] imageType = { "jpg", "jpeg", "png", "gif" };
        public static readonly string[] docType = { "doc", "docx", "pdf", "xls", "xlsx", "ppt", "pptx", "txt", "zip", "rar" };
        public static readonly string[] mediaType = { "mp4", "mp3", "aac" };
        public UploadFileService(IConfiguration configuration,
            IWebHostEnvironment environment,
            IOptions<ConnectionStrings> connectionStrings)
        {
            _configuration = configuration;
            _environment = environment;
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public async Task<dynamic> UploadFiles(IFormFile vmFile)
        {
            var dateF = DateTime.Now.ToString("yyyyMM");
            var url = $"\\Upload\\Documents\\{dateF}";
            var urlRes = $"/Upload/Documents/{dateF}/";
            var fileType = Path.GetExtension(vmFile.FileName.ToLowerInvariant()).Substring(1);//get file type
            if (imageType.Any(x => x == fileType))
            {
                url = $"\\Upload\\Images\\{dateF}";
                urlRes = $"/Upload/Images/{dateF}/";
            }
            else if (docType.Any(x => x == fileType))
            {
                url = $"\\Upload\\Documents\\{dateF}";
                urlRes = $"/Upload/Documents/{dateF}/";
            }
            else if (mediaType.Any(x => x == fileType))
            {
                url = $"\\Upload\\Media\\{dateF}";
                urlRes = $"/Upload/Media/{dateF}/";
            }

            if (!Directory.Exists(_environment.WebRootPath + url))
            {
                Directory.CreateDirectory(_environment.WebRootPath + $"{url}\\");
            }
            using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + $"{url}\\" + vmFile.FileName))
            {
                await vmFile.CopyToAsync(filestream);
                filestream.Flush();
            }
            if (File.Exists(_environment.WebRootPath + $"{url}\\" + vmFile.FileName))
            {
                return urlRes + vmFile.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
