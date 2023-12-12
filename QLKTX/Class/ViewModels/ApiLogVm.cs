namespace QLKTX.Class.ViewModels
{
    public class ApiLogVm
    {
        #region Public Properties
        public Guid LogID { get; set; }

        public string Host { get; set; }

        public string RequestHeaders { get; set; }

        public string StatusCode { get; set; }

        public DateTime TimeUtc { get; set; }

        public string RequestBody { get; set; }

        public string RequestedMethod { get; set; }

        public string UserHostAddress { get; set; }

        public string AbsoluteUri { get; set; }

        public string RequestType { get; set; }

        public string AccountLogin { get; set; }

        public string TransactionID { get; set; }

        public string TransactionNo { get; set; }

        public string FormID { get; set; }
        #endregion
    }
}
