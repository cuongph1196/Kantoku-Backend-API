namespace QLKTX.Class.Entities
{
    public class DebtDetail
    {
        #region Public Properties
        public int? DetailRowkey { get; set; }

        public int? MasterRowkey { get; set; }

        public int? ReasonKey { get; set; }
        public string ReasonName { get; set; }

        public string Description { get; set; }

        public decimal InAmount { get; set; }

        public decimal OutAmount { get; set; }
        #endregion
    }
}
