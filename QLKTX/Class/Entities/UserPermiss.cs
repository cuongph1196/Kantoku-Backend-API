namespace QLKTX.Class.Entities
{
    public class UserPermiss
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int UserGroupID { get; set; }
        public string UserGroupName { get; set; }
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public bool FView { get; set; }
        public bool FAdd { get; set; }
        public bool FEdit { get; set; }
        public bool FDel { get; set; }
        public bool FApp { get; set; }
        public bool FReject { get; set; }
        public int LevelKey { get; set; }
    }
}
