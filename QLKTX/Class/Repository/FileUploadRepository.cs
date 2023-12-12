using Microsoft.Extensions.Options;
using QLKTX.Class.ViewModels;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IFileUploadRepository
    {
        dynamic GetByKey(string transID, int masterKey, int? detailKey);
        dynamic GetById(int id);
        dynamic Create(FileUploadVm vm);
        dynamic Delete(int id);
        dynamic DeleteByMasterKey(int key, string transID);
    }
    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;
        public static readonly string[] imageType = { "jpg", "jpeg", "png", "gif" };
        public static readonly string[] docType = { "doc", "docx", "pdf", "xls", "xlsx", "ppt", "pptx", "txt", "zip", "rar" };
        public static readonly string[] mediaType = { "mp4", "mp3", "aac" };
        
        public FileUploadRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _configuration = configuration;
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic GetByKey(string transID, int masterKey, int? detailKey)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"TransID", transID},
                {"MasterKey", masterKey},
                {"DetailKey", detailKey}
            };
            var result = new StoredProcedureFactory<FileUploadVm>(_connectionString).FindAllBy(masterParams,
                "spfrm_FileUploadSearch", "GetAllByKey");
            return result;
        }

        public dynamic GetById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", id}
            };
            var result = new StoredProcedureFactory<FileUploadVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_FileUploadSearch", "GetByID");
            return result;
        }

        public dynamic Create(FileUploadVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"TransID", vm.TransID},
                {"MasterKey", vm.MasterKey},
                {"DetailKey", vm.DetailKey},
                {"FileName", vm.FileName},
                {"FileType", vm.FileType},
                {"FileUrl", vm.FileUrl}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_FileUpload", "Create");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_FileUpload", "Delete");
            return result;
        }

        public dynamic DeleteByMasterKey(int key, string transID)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterKey", key},
                {"TransID", transID}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_FileUpload", "DeleteByMasterKey");
            return result;
        }
    }
}
