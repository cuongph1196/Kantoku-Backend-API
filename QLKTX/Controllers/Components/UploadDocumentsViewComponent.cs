using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;

namespace QLKTX.Controllers.Components
{
    public class UploadDocumentsViewComponent : ViewComponent
    {
        private readonly IFileUploadRepository _fileUploadRepository;
        public UploadDocumentsViewComponent(IFileUploadRepository fileUploadRepository)
        {
            _fileUploadRepository = fileUploadRepository;
        }

        public IViewComponentResult Invoke(string transID, int masterkey, int? detaiKey, bool isDel = true)
        {
            var resultData = new UploadDocumentsViewModel();
            resultData.isDel = isDel;
            if (masterkey > 0)
            {
                var result = _fileUploadRepository.GetByKey(transID, masterkey, detaiKey);
                if (result.Success)
                {
                    resultData.Documents = (List<FileUploadVm>)result.Data.Items;
                }
            }
            return View(resultData);
        }

    }
}
