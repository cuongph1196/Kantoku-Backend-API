using Dapper;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX.Class.Helper
{
    public class StoredProcedureFactory<T>
    {
        private readonly string _connectionString;

        public StoredProcedureFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region "Sql Store procedure"

        //void Execute no return
        public void voidExecute(Dictionary<string, object> parameters, string spname, string activity)
        {
            try
            {
                if (_connectionString != "")
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        if (conn.State == System.Data.ConnectionState.Closed)
                            conn.Open();
                        var param = SetParameters(parameters);
                        //default
                        param.Add("@Activity", activity);
                        param.Add("@AccountLogin", Mession.AccountLogin);
                        conn.Execute(spname, param, commandType: CommandType.StoredProcedure);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //Execute ra int
        public ApiResult<int> intExecute(Dictionary<string, object> parameters, string spname, string activity)
        {
            int intResult = 0;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<int>(-1); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    param.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.Output, size: int.MaxValue);
                    conn.Execute(spname, param, commandType: CommandType.StoredProcedure);
                    intResult = param.Get<int>("@ReturnID");
                    if (intResult > 0)
                        return new ApiSuccessResult<int>(intResult);
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(-1); //Exception
            }
            return new ApiErrorResult<int>(intResult);
        }

        //Execute ra string
        public ApiResult<string> msgExecute(Dictionary<string, object> parameters, string spname, string activity)
        {
            string msgResult = "";
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<string>("E_EXC");
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    conn.Execute(spname, param, commandType: CommandType.StoredProcedure);
                    msgResult = param.Get<string>("@ReturnMess");
                    if (msgResult == "SUCCESS")
                        return new ApiSuccessResult<string>(msgResult);
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("E_EXC");
            }
            return new ApiErrorResult<string>(msgResult);
        }

        //Execute ra string ID
        public ApiResult<string> msgExecuteReturnStrID(Dictionary<string, object> parameters, string spname, string activity)
        {
            string strIdResult = "";
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    param.Add("ReturnStrID", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                    conn.Execute(spname, param, commandType: CommandType.StoredProcedure);
                    strIdResult = param.Get<string>("@ReturnStrID");
                    string msgResult = param.Get<string>("@ReturnMess");
                    if (!string.IsNullOrEmpty(strIdResult))
                    {
                        return new ApiSuccessResult<string>(strIdResult);
                    }
                    else if (!string.IsNullOrEmpty(msgResult))
                    {
                        return new ApiErrorResult<string>(msgResult);
                    }
                }
            }
            catch (Exception)
            {
                return new ApiErrorResult<string>("E_EXC");
            }
            return new ApiErrorResult<string>("F_ERROR");
        }

        //Query ra string
        public ApiResult<string> msgQueryFirstOrDefault(Dictionary<string, object> parameters, string spname, string activity)
        {
            string msgResult = "";
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<string>("E_EXC");
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    //lấy string ko nên dùng QuerySingle vì exception, dùng QueryFirstOrDefault return null
                    msgResult = conn.QueryFirstOrDefault<string>(spname, param, null, null, CommandType.StoredProcedure);
                    if (msgResult != null)
                        return new ApiSuccessResult<string>(msgResult);
                }
            }
            catch (Exception)
            {
                return new ApiErrorResult<string>("E_EXC");
            }
            return new ApiErrorResult<string>(msgResult);
        }

        //Query ra bool
        public ApiResult<bool> boolQueryFirstOrDefault(Dictionary<string, object> parameters, string spname, string activity)
        {
            bool msgResult = false;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<bool>(msgResult);
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    //lấy string ko nên dùng QuerySingle vì exception, dùng QueryFirstOrDefault return null
                    msgResult = conn.QueryFirstOrDefault<bool>(spname, param, null, null, CommandType.StoredProcedure);
                    if (msgResult)
                        return new ApiSuccessResult<bool>(msgResult);
                }
            }
            catch (Exception)
            {
                return new ApiErrorResult<bool>(msgResult);
            }
            return new ApiErrorResult<bool>(msgResult);
        }

        public ApiResult<T> FindOneBy(Dictionary<string, object> parameters, string spname, string activity)
        {
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<T>("E_EXC"); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    var entity = conn.Query<T>(spname, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (entity != null)
                    {
                        return new ApiSuccessResult<T>(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<T>("E_EXC"); //Exception
            }
            return new ApiErrorResult<T>("F_ID_NEXIST"); //NOT FOUND
        }

        public ApiResult<PageReponResult<T>> FindAllBy(Dictionary<string, object> parameters, string spname, string activity)
        {
            var pagedResult = new PageReponResult<T>();
            pagedResult.TotalRow = 0;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    var datas = conn.Query<T>(spname, param, commandType: CommandType.StoredProcedure).ToList(); //ToList() performance tốt hơn Enumable
                    if (datas.Any())
                    {
                        pagedResult.Items = datas;
                        pagedResult.TotalRow = datas.Count();
                        return new ApiSuccessResult<PageReponResult<T>>(pagedResult);
                    }
                }
            }
            catch (Exception e)
            {
                return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
            }
            return new ApiErrorResult<PageReponResult<T>>("F_ID_NEXIST"); //NOT FOUND
        }

        public ApiResult<PageReponResult<T>> FindAllByCommandTimeout(Dictionary<string, object> parameters, string spname, string activity)
        {
            var pagedResult = new PageReponResult<T>();
            pagedResult.TotalRow = 0;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    int? parameterForTimeout = 300; //5p
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    var datas = conn.Query<T>(spname, param, commandTimeout: parameterForTimeout, commandType: CommandType.StoredProcedure).ToList(); //ToList() performance tốt hơn Enumable
                    if (datas.Any())
                    {
                        pagedResult.Items = datas;
                        pagedResult.TotalRow = datas.Count();
                        return new ApiSuccessResult<PageReponResult<T>>(pagedResult);
                    }
                }
            }
            catch (Exception)
            {
                return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
            }
            return new ApiErrorResult<PageReponResult<T>>("F_ID_NEXIST"); //NOT FOUND
        }

        public ApiResult<PageReponResult<T>> FindComboboxBy(Dictionary<string, object> parameters, string spname, string activity)
        {
            var pagedResult = new PageReponResult<T>();
            pagedResult.TotalRow = 0;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters_Combo(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    var datas = conn.Query<T>(spname, param, commandType: CommandType.StoredProcedure).ToList(); //ToList() performance tốt hơn Enumable
                    if (datas.Any())
                    {
                        pagedResult.Items = datas;
                        pagedResult.TotalRow = datas.Count();
                        return new ApiSuccessResult<PageReponResult<T>>(pagedResult);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
            }
            return new ApiErrorResult<PageReponResult<T>>("F_ID_NEXIST"); //NOT FOUND
        }

        public ApiResult<PageReponResult<T>> FindCombotreeBy(Dictionary<string, object> parameters, string spname, string activity)
        {
            var pagedResult = new PageReponResult<T>();
            pagedResult.TotalRow = 0;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters_Combo(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    var datas = conn.Query<T>(spname, param, commandType: CommandType.StoredProcedure).ToList(); //ToList() performance tốt hơn Enumable
                    if (datas.Any())
                    {
                        pagedResult.Items = datas;
                        pagedResult.TotalRow = datas.Count();
                        return new ApiSuccessResult<PageReponResult<T>>(pagedResult);
                    }
                }
            }
            catch (Exception)
            {
                return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
            }
            return new ApiErrorResult<PageReponResult<T>>("F_ID_NEXIST"); //NOT FOUND
        }

        public ApiResult<PageReponResult<T>> FindAutocomplexBy(Dictionary<string, object> parameters, string spname, string activity)
        {
            var pagedResult = new PageReponResult<T>();
            pagedResult.TotalRow = 0;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters_Combo(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    var datas = conn.Query<T>(spname, param, commandType: CommandType.StoredProcedure).ToList(); //ToList() performance tốt hơn Enumable
                    if (datas.Any())
                    {
                        pagedResult.Items = datas;
                        pagedResult.TotalRow = datas.Count();
                        return new ApiSuccessResult<PageReponResult<T>>(pagedResult);
                    }
                }
            }
            catch (Exception)
            {
                return new ApiErrorResult<PageReponResult<T>>("E_EXC"); //Exception
            }
            return new ApiErrorResult<PageReponResult<T>>("F_ID_NEXIST"); //NOT FOUND
        }


        //Async
        //Execute ra int
        public async Task<ApiResult<int>> intExecuteAsync(Dictionary<string, object> parameters, string spname, string activity)
        {
            int intResult = 0;
            try
            {
                if (_connectionString == "")
                {
                    return new ApiErrorResult<int>(-1); //Exception
                }
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    var param = SetParameters(parameters);
                    //default
                    param.Add("@Activity", activity);
                    param.Add("@AccountLogin", Mession.AccountLogin);
                    param.Add("@ReturnID", dbType: DbType.Int32, direction: ParameterDirection.Output, size: int.MaxValue);
                    await conn.ExecuteAsync(spname, param, commandType: CommandType.StoredProcedure);
                    intResult = param.Get<int>("@ReturnID");
                    if (intResult > 0)
                        return new ApiSuccessResult<int>(intResult);
                }
            }
            catch (Exception)
            {
                return new ApiErrorResult<int>(-1); //Exception
            }
            return new ApiErrorResult<int>(intResult);
        }

        public DataTable GetDataTable(ArrayList arrParams, string storeName, string activity)
        {
            if (_connectionString == "")
            {
                return null; //Exception
            }
            //load data table dùng param
            SqlCommand cmd = new SqlCommand();
            SqlConnection SQLconn = new SqlConnection(_connectionString);
            DataTable dt = new DataTable();
            SqlDataAdapter adpAdapter = new SqlDataAdapter();
            try
            {
                if (SQLconn.State != ConnectionState.Open)
                    SQLconn.Open();
                cmd.Connection = SQLconn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeName;
                cmd.CommandTimeout = 60 * 10; //hunglt bỏ * 1000 chỉ cho 10p thôi
                cmd.Parameters.Clear();
                //Add Params
                foreach (SqlParameter param in arrParams)
                {
                    cmd.Parameters.Add(param);
                }
                cmd.Parameters.Add(new SqlParameter("@Activity", activity));
                adpAdapter.SelectCommand = cmd;
                adpAdapter.Fill(dt);

                cmd.Dispose();
                cmd.Parameters.Clear();
                SQLconn.Close();
                SQLconn.Dispose();
                adpAdapter.Dispose();
                return dt;
            }
            catch (Exception exp)
            {
                cmd.Dispose();
                cmd.Parameters.Clear();
                SQLconn.Close();
                SQLconn.Dispose();
                adpAdapter.Dispose();
                string strErr = exp.Message;
                return null;
            }
        }
        #endregion "Sql Store procedure"

        #region "Private function"

        private DynamicParameters SetParameters(Dictionary<string, object> parameters)
        {
            var param = new DynamicParameters();
            foreach (var key in parameters.Keys)
            {
                param.Add("@" + key, parameters[key]);
            }
            param.Add("ReturnMess", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return param;
        }

        private DynamicParameters SetParameters_Combo(Dictionary<string, object> parameters)
        {
            var param = new DynamicParameters();
            if (!parameters.ContainsKey("Active"))
            {
                param.Add("@Active", null);
            }
            foreach (var key in parameters.Keys)
            {
                param.Add("@" + key, parameters[key]);
            }
            return param;
        }

        #endregion "Private function"
    }
}
