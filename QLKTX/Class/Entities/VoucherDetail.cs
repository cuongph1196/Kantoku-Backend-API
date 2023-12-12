namespace QLKTX.Class.Entities
{
    public class VoucherDetail
    {
        #region Public Properties
        public int? DetailRowkey { get; set; }

        public int? MasterRowkey { get; set; }

        public int? ReasonKey { get; set; }
        public string ReasonName { get; set; }

        public string Description { get; set; }

        public decimal InAmount { get; set; }

        public decimal OutAmount { get; set; }
        public int ContractKey { get; set; }
        public int ContractDetailKey { get; set; }
        public string DepartmentKey { get; set; }
        public IEnumerable<string> DepartmentKeys
            => string.IsNullOrEmpty(DepartmentKey)
                ? null
                : DepartmentKey.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        public string DepartmentName { get; set; }
        #endregion
    }
}
