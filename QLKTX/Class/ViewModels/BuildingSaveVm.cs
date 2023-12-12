namespace QLKTX.Class.ViewModels
{
    public class BuildingSaveVm
    {
        public BuildingVm Building { get; set; }
        public List<BuildingSectionVm> BuildingSections { get; set; }

        //public BuildingSaveVm()
        //{
        //    BuildingSections = new List<BuildingSectionVm>();
        //}
    }
}
