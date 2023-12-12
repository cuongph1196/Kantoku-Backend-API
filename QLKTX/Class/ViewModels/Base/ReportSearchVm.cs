namespace QLKTX.Class.ViewModels.Base
{
    public class ReportSearchVm
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RenderType { get; set; }
        public int? BuildingKey { get; set; }
        public int? BuildingSectionKey { get; set; }
        public int? PartnerKey { get; set; }
        public int FunctionID { get; set; }
    }
}
