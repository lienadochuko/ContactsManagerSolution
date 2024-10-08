using Contact_Manager.Filters.ActionFilters;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Exceptions;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin, Developer")]
    public class MoviesController(IMoviesGetterServices moviesGetterServices,
        IMoviesUpdaterServices moviesUpdaterServices, ILogger<MoviesController> logger) : Controller
    {
        [Route("[action]")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            IEnumerable<ActorResponse> Actors = await moviesGetterServices.GetAllActors(cancellationToken);

            return View(Actors);
        }

        [HttpGet]
        [Route("[action]/{actorsID}")] //Url: person/edit/1
        public async Task<IActionResult> Edit(string actorsID, CancellationToken cancellationToken)
        {
            ActorResponse? actorResponse = await moviesGetterServices.GetActorsByID(actorsID, cancellationToken);
            if (actorResponse == null)
            {
                return RedirectToAction("Index", "Movies");
            }

            ActorsUpdateRequest actorsUpdateRequest = actorResponse.ToActorsUpdateRequest();

            return View(actorsUpdateRequest);
        }


        [HttpPost]
        [Route("[action]/{actorsID}")]
        public async Task<IActionResult> Edit(ActorsUpdateRequest actorsUpdateRequest, CancellationToken cancellationToken)
        {

            if (actorsUpdateRequest.ActorID == null)
            {
                return RedirectToAction("Index", "Movies");
            }

            ActorResponse Actors = await moviesGetterServices.GetActorsByID(actorsUpdateRequest.ActorID, cancellationToken);

            if (Actors == null)
            {
                throw new InvalidPersonIDException("Actor does not existed");
            }

           var isUpdated = await moviesUpdaterServices.UpdateActors(actorsUpdateRequest, cancellationToken);

            if (isUpdated)
            {
                return RedirectToAction("Index", "Movies");
            }
            else
            {
                throw new Exception("Failed to update Actor's details");
            }
        }


    }
}
