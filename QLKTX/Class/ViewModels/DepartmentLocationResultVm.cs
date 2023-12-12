namespace QLKTX.Class.ViewModels
{
    public class DepartmentLocationResultVm
    {
        public string BuildSImage { get; set; }
        public List<dynamic> DeptLocations { get; set; }

        public DepartmentLocationResultVm()
        {
            DeptLocations = new List<dynamic>();
        }
    }
}
