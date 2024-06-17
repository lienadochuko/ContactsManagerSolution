using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Contact_Manager.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        public readonly ICountriesService _countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        [Route("UploadExcel")]
        [HttpGet]
        public IActionResult UploadExcel()
        {
            return View();
        }

        [Route("UploadExcel")]
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported fileType, Please select an xlsx file";
                return View();
            }
            
          int count = await _countriesService.UploadCountriesFromExcelFile(excelFile);

            ViewBag.Message = $"{count} Countries Uploaded";
            return View();
        }
    }
}
