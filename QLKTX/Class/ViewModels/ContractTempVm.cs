namespace QLKTX.Class.ViewModels
{
    public class ContractTempVm
    {
        #region Public Properties
        public int? ContractTempKey { get; set; }

        public int? ContractKey { get; set; }

        public int? ContractDeclareDetailKey { get; set; }

        public string Description { get; set; }

        public bool? Status { get; set; }

        public int DebtReasonKey { get; set; }
        public string DebtReasonName { get; set; }

        public decimal Amount { get; set; }
        public decimal? AmountD { get; set; }

        public bool Active { get; set; }

        public decimal? DepositMonth { get; set; }

        public decimal? PenaltyFee { get; set; }

        public decimal? PenaltyFeeAfterDay { get; set; }

        public decimal? IncreaseRentAmount { get; set; }

        public decimal? IncreaseRentPerior { get; set; }

        public int? PaymentPeriod { get; set; }

        public int? PaymentDate { get; set; }

        public decimal? DepositMonthD { get; set; }

        public decimal? PenaltyFeeD { get; set; }

        public decimal? PenaltyFeeAfterDayD { get; set; }

        public decimal? IncreaseRentAmountD { get; set; }

        public decimal? IncreaseRentPeriorD { get; set; }

        public int? PaymentPeriodD { get; set; }

        public int? PaymentDateD { get; set; }
        #endregion
    }
}
