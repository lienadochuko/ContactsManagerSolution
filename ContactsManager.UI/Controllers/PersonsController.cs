using Contact_Manager.Filters;
using Contact_Manager.Filters.ActionFilters;
using Contact_Manager.Filters.AlwaysRunResultFilter;
using Contact_Manager.Filters.Authorization_Filter;
using Contact_Manager.Filters.ExceptionFilters;
using Contact_Manager.Filters.ResourceFilters;
using Contact_Manager.Filters.ResultFilters;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.Style;
using Rotativa.AspNetCore;

namespace Contact_Manager.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin, Developer")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-From-Controller-Key", "My-From-Controller-Value", 1 })]
    [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]
    [TypeFilter(typeof(HandleExceptionFilter))]
	public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonGetterServices _personGetterServices;
        private readonly IPersonAdderServices _personAdderServices;
        private readonly IPersonSorterServices _personSorterServices;
        private readonly IPersonUpdaterServices _personUpdaterServices;
        private readonly IPersonDeleterServices _personDeleterServices;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

	//contructor
	public PersonsController(
        IPersonGetterServices personGetterServices,
        IPersonAdderServices personAdderServices,
        IPersonSorterServices personSorterServices,
        IPersonUpdaterServices personUpdaterServices,
        IPersonDeleterServices personDeleterServices,
        ICountriesService countriesService, 
        ILogger<PersonsController> logger)
        {
            _personGetterServices = personGetterServices;
            _personDeleterServices = personDeleterServices;
            _personUpdaterServices = personUpdaterServices;
            _personAdderServices = personAdderServices;
            _personSorterServices = personSorterServices;
            _countriesService = countriesService;
            _logger = logger;
        } 


        //Url: persons/index
        [Route("[action]")]
        //Url: persons/
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter))]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        public async Task<IActionResult> Index(string searchBy, string? searchString, 
            string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrderOptions = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index Action for personController");
            _logger.LogDebug($"searhBy: {searchBy}, searchString: {searchString}, " +
                $"sortBy: {sortBy}, sortOrderOptions: {sortOrderOptions}");

            
            List<PersonResponse> persons = await _personGetterServices.GetFilteredPersons(searchBy, searchString);
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPerson = await _personSorterServices.GetSortedPersons(persons, sortBy, sortOrderOptions);
            //ViewBag.CurrentSortBy = sortBy.ToString();
            //ViewBag.CurrentSortOrderOptions = sortOrderOptions.ToString();
            return View(sortedPerson);
        }

        //Url: persons/create
        [Route("[action]")]
        [HttpGet] //indicates that the action recieves only get requests
        [TypeFilter(typeof(TokenResultFilter))]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Key", "X-Value", 5 })]
        //[AttributeHeaderActionFilter( "X-Key", "X-Value", 5 )]
        //[ResponseHeaderFilterFactory( "X-Key", "X-Value", 5 )]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString(),
            });

            return View();
        }

        //Url: persons/create
        [Route("[action]")]
        [HttpPost] //indicates that the action recieves only post requests
        [TypeFilter(typeof(PersonCreateandEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        [TypeFilter(typeof(FeatureDisableldResourceFilter), Arguments = new object[] {false})]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            //if (!ModelState.IsValid)
            //{
            //    List<CountryResponse> countries = await _countriesService.GetAllCountries();
            //    ViewBag.Countries = countries.Select(temp =>
            //    new SelectListItem()
            //    {
            //        Text = temp.CountryName,
            //        Value = temp.CountryID.ToString(),
            //    });

            //    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            //    return View(personRequest);
            //}

            //call the service method
            PersonResponse personResponse = await _personAdderServices.AddPerson(personRequest);

            //navigate to Index() action method (it make another get request to "persons/index")
            return RedirectToAction("Index", "Persons");
        }


        [HttpGet]
        [Route("[action]/{personID}")] //Url: person/edit/1
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personGetterServices.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString(),
            });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateandEditPostActionFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse = await _personGetterServices.GetPersonByPersonID(personRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            //if (ModelState.IsValid)
            //{
                await _personUpdaterServices.UpdatePerson(personRequest);
                return RedirectToAction("Index", "Persons");
            //}
            //else
            //{
            //    List<CountryResponse> countries = await _countriesService.GetAllCountries();
            //    ViewBag.Countries = countries;

            //    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    return View();
            //}
        }


        [HttpGet]
        [Route("[action]/{personID}")] //Url: person/delete/1
        public async Task<IActionResult> Delete(Guid personID)
        {
            PersonResponse? personResponse = await _personGetterServices.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            //_personServices.DeletePerson(personID);

            return View(personResponse);
        }


        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonResponse personResponse)
        {
            if (personResponse == null)
                return RedirectToAction("Index");

            await _personDeleterServices.DeletePerson(personResponse.PersonID);
            return RedirectToAction("Index");
        }

        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            //Get list of persons
            List<PersonResponse> personList = await _personGetterServices.GetAllPersons();

            //Return View as PDF
            return new ViewAsPdf("PersonsPDF", personList, ViewData)
            {
                FileName = "MyPdfFile.pdf",
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape //it is by default Potrait
            };
        }
        
        [Route("PersonalPDF")]
        public async Task<IActionResult> PersonalPDF()
        {
            //Get list of persons
            List<PersonResponse> personList = await _personGetterServices.GetAllPersons();

            //Return View as PDF
            return new ViewAsPdf("PersonalPDF", personList, ViewData)
            {
                FileName = "MyPdfFile.pdf",
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape //it is by default Potrait
            };
        }

        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
           MemoryStream memoryStream = await _personGetterServices.GetPersonCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personGetterServices.GetPersonExcel();

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }

        [Route("PersonUpload")]
        [HttpGet]
        public IActionResult UploadExcel()
        {
            return View();
        }

        [Route("PersonUpload")]
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

            int count = await _personAdderServices.UploadPersonsFromExcelFile(excelFile);

            ViewBag.Message = $"{count} Person Uploaded";
            return View();
        }
    }
}
