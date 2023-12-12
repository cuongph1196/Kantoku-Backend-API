using Microsoft.Extensions.Options;
using QLKTX.Class.Dtos;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IAutoTransNoRepository
    {
        string GetAutoTransNo(string transID, string transDate);
    }
    public class AutoTransNoRepository : IAutoTransNoRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public AutoTransNoRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public string GetAutoTransNo(string transID, string transDate)
        {
            string strTransNo = "N/A";
            var result = new ApiResult<string>();
            try
            {
                var dictParams = new Dictionary<string, object>();
                dictParams.Add("TransID", transID);
                dictParams.Add("TransDate", transDate);
                result = new StoredProcedureFactory<string>(_connectionString).msgQueryFirstOrDefault(dictParams, "sp_clsGetAutoTransNo", "GetAutoTransNo");
                if (result.Success)
                {
                    strTransNo = result.Message; //lấy ra TranNo
                }
            }
            catch (Exception)
            {
                strTransNo = "N/A";
            }
            return strTransNo;
        }
    }
}
