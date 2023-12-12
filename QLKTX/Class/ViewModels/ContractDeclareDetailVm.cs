namespace QLKTX.Class.ViewModels
{
    public class ContractDeclareDetailVm
    {
        #region Public Properties
        public int? ContractDeclareDetailKey { get; set; }

        public int? ContractDeclareKey { get; set; }

        public int? DebtReasonKey { get; set; }
        public string DebtReasonName { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public decimal DepositMonth { get; set; }

        public decimal PenaltyFee { get; set; }

        public decimal PenaltyFeeAfterDay { get; set; }

        public decimal IncreaseRentAmount { get; set; }

        public decimal IncreaseRentPerior { get; set; }
        public int PaymentPeriod { get; set; }
        public int PaymentDate { get; set; }
        #endregion
    }
}
