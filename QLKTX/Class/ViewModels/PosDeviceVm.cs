namespace QLKTX.Class.ViewModels
{
    public class PosDeviceVm
    {
        #region Public Properties
        public int? DeviceKey { get; set; }

        public string DeviceCode { get; set; }

        public string Device_Seri { get; set; }

        public bool Active { get; set; } = false; //default

        public string DeviceName { get; set; }

        public string DeviceDesc { get; set; }
        #endregion
    }
}
