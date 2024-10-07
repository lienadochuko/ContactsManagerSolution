using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin, Developer")]
    public class MoviesController(IMoviesGetterServices moviesGetterServices, ILogger<MoviesController> logger) : Controller
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
    }
}
