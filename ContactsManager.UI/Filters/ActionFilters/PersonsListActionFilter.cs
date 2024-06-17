using Contact_Manager.Controllers;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contact_Manager.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        public readonly ILogger<PersonsListActionFilter> Logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger) { Logger = logger; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To Do: add after logic here
            Logger.LogInformation("{FilterName}.{MethodName} method", 
                nameof(PersonsListActionFilter), nameof(OnActionExecuted));

            PersonsController personsController = (PersonsController) context.Controller;

            IDictionary<string, object?>? parameters = 
               (IDictionary<string, object?>) context.HttpContext.Items["arguments"];
            

            if(parameters != null)
            {
                if(parameters.ContainsKey("searchBy"))
                {
                   personsController.ViewData["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);
                }

                if (parameters.ContainsKey("searchString"))
                {
                    personsController.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
                }
                if (parameters.ContainsKey("sortBy"))
                {
                    personsController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortBy"] = nameof(PersonResponse.PersonName);
                }


                if (parameters.ContainsKey("sortOrderOptions"))
                {
                    personsController.ViewData["CurrentSortOrderOptions"] = Convert.ToString(parameters["sortBy"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortOrderOptions"] = nameof(SortOrderOptions.ASC);
                }
            }

            //Searching
            personsController.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name"},
                {nameof(PersonResponse.Email), "Email"},
                {nameof(PersonResponse.DOB), "Date of Birth"},
                {nameof(PersonResponse.Gender), "Gender"},
                {nameof(PersonResponse.Address), "Address"},
                {nameof(PersonResponse.CountryID), "Country"}
            };
        } 

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            //To Do: add before logic here
            Logger.LogInformation("PersonListActionFilter.OnActionExecuting method");

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                
                //Validate the searchBy Parameter value 
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOption = new List<string>()
                    {
                        
                        nameof(PersonResponse.DOB),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.Address),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.PersonName),
                    };

                    if (searchByOption.Any(temp => temp == searchBy) == false)
                    {
                        Logger.LogInformation("searchBy actual value "+searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    }
                    else
                    {
                        Logger.LogInformation("searchBy actual value "+searchBy);
                    }
                }
            }
        }
    }
}
