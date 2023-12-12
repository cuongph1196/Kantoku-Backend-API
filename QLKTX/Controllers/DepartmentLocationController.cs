using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using System.Reflection;

namespace QLKTX.Controllers
{
    [Route("DepartmentLocation")]
    [ApiController]
    [Authorize]
    public class DepartmentLocationController : BaseController
    {
        private readonly ILogger<DepartmentLocationController> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentLocationRepository _departmentLocationRepository;
        private readonly IBuildingRepository _buildingRepository;
        public static readonly string[] imageType = { "jpg", "jpeg", "png", "gif" };

        public DepartmentLocationController(ILogger<DepartmentLocationController> logger,
            IDepartmentRepository departmentRepository,
            IDepartmentLocationRepository departmentLocationRepository,
            IBuildingRepository buildingRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _departmentLocationRepository = departmentLocationRepository;
            _buildingRepository = buildingRepository;
        }

        public IActionResult Index(int ModuleID, int FunctionID)
        {
            var funcS = HttpContext.Session.GetString(SystemConstants.FunctionMenuSession + "_" + ModuleID);
            var funcMenu = new List<FunctionMenuVm>();
            if (funcS != null)
            {
                funcMenu = JsonConvert.DeserializeObject<List<FunctionMenuVm>>(funcS);
            }
            var objInfo = new PageInfoViewModel();
            objInfo.ModuleID = ModuleID;
            objInfo.FunctionID = FunctionID;
            objInfo.FunctionName = funcMenu.Find(x => x.FunctionID == FunctionID)?.FunctionName;
            objInfo.AccountLogin = LoggedInUser.UserID;

            return View(objInfo);
        }

        [Route("GetLocation")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetLocation([FromQuery] DepartmentLocationSearchVm vm)
        {
            var resultData = new DepartmentLocationResultVm();
            var result = _buildingRepository.GetDetailImageByID(vm.BuildingSectionKey);
            if (result.Success)
            {
                resultData.BuildSImage = result.Data?.BuildingSectionImg;
                var resultL = _departmentLocationRepository.GetAllLocation(vm);
                if(resultL.Success)
                    resultData.DeptLocations = resultL.Data?.Items;

                return Ok(new ApiSuccessResult<DepartmentLocationResultVm>(resultData));
            }
            return Ok(new ApiErrorResult<string>());
        }

        [Route("GetByID/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetByID([FromRoute] int id)
        {
            var result = _departmentLocationRepository.GetByID(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] DepartmentLocationVm model)
        {
            var result = _departmentLocationRepository.Create(model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] DepartmentLocationVm model)
        {
            var result = _departmentLocationRepository.Update(model);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var result = _departmentLocationRepository.Delete(id);
            return Ok(result);
        }
        
        [Route("DeleteByBuild")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteByBuild([FromBody] DepartmentLocationDelVm vm)
        {
            var objBSI = new BuildingSectionImgVm()
            {
                BuildingSectionKey = vm.BuildingSectionKey,
                BuildingSectionImg = null
            };
            var resultImg = _buildingRepository.UpdateDetailImg(objBSI);
            if (resultImg.Success)
            {
                var resultL = _departmentLocationRepository.DeleteByBuild(vm);
                if (!resultL.Success) return Ok(new ApiErrorResult<int>());
            }
            return Ok(resultImg);
        }

        [Route("CreateDepartment")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        public IActionResult CreateDepartment([FromBody] DepartmentCreateVm model)
        {
            var result = _departmentRepository.Create(model.Department);
            if (result.Success)
            {
                model.Location.DepartmentKey = result.Data;
                var resultLocation = _departmentLocationRepository.Create(model.Location);
                if (!resultLocation.Success) return Ok(new ApiErrorResult<int>());
            }
            return Ok(result);
        }

        [Route("UploadBuildingImg")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadBuildingImg()
        {
            var currentRequest = HttpContext.Request;
            if (currentRequest.Form.Files.Count > 0)
            {
                var dict = currentRequest.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                var buildSKey = string.Empty;
                foreach (var item in dict)
                {
                    if (item.Key == "BuildingSectionKey")
                    {
                        buildSKey = item.Value;
                        continue;
                    }
                }
                if (buildSKey != string.Empty)
                {
                    IFormFile vmFile = HttpContext.Request.Form.Files.GetFile("Image");
                    //// Loop through uploaded files  
                    //for (var i = 0; i < currentRequest.Form.Files.Count; i++)
                    //{
                    //    IFormFile httpPostedFile = currentRequest.Form.Files[i];
                    //    vmFile.Add(httpPostedFile);
                    //}
                    var fileType = Path.GetExtension(vmFile.FileName.ToLowerInvariant()).Substring(1);//get file name
                    if (imageType.Any(x => x == fileType))
                    {
                        using var dataStream = new MemoryStream();
                        await vmFile.CopyToAsync(dataStream);
                        byte[] fileBytes = dataStream.ToArray(); // you can save this to your byte array variable and remove the 2 lines below
                        string base64String = Convert.ToBase64String(fileBytes);

                        var objBSI = new BuildingSectionImgVm()
                        {
                            BuildingSectionKey = int.Parse(buildSKey),
                            BuildingSectionImg = base64String
                        };
                        var result = _buildingRepository.UpdateDetailImg(objBSI);
                        return Ok(result);
                    }
                }
                return Ok(new ApiErrorResult<string>());
            }
            return Ok(new ApiErrorResult<string>("Không có files"));
        }


    }
}
