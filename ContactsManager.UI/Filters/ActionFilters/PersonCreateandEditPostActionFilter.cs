using Contact_Manager.Controllers;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Contact_Manager.Filters.ActionFilters
{
    public class PersonCreateandEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<PersonCreateandEditPostActionFilter> _logger;
        private readonly ICountriesService _countriesService;

        public PersonCreateandEditPostActionFilter(ICountriesService countriesService, ILogger<PersonCreateandEditPostActionFilter> logger)
        {
            _logger = logger;
            _countriesService = countriesService;
            //Order = order;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before
            if (context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countriesService.GetAllCountries();
                    personsController.ViewBag.Countries = countries.Select(temp =>
                    new SelectListItem()
                    {
                        Text = temp.CountryName,
                        Value = temp.CountryID.ToString(),
                    });

                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    var personRequest = context.ActionArguments["personRequest"];

                    context.Result = personsController.View(personRequest);//short-circuits or skips the subsequent action filters & action Methods
                }
                else
                {
                    await next();//calls the subsequent filter or action method
                }
            }
            else
            {
                await next();//calls the subsequent filter or action method
            }
            //after
            _logger.LogInformation("After Logic of PersonCreateandEditPostActionFilter");
        }
    }
}
