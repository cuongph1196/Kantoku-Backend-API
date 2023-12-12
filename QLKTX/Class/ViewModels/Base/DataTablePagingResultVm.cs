namespace QLKTX.Class.ViewModels.Base
{
    public class DataTablePagingResultVm<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }

        public DataTablePagingResultVm()
        {
            data = new List<T>();
        }
    }
}
