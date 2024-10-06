using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin, Developer")]
    public class MoviesController(IMoviesRepository moviesRepository, ILogger<MoviesController> logger) : Controller
    {
        [Route("[action]")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var Actors = await moviesRepository.GetActors(cancellationToken);

            IEnumerable<ActorResponse> ActorsResponse = Actors.Select(select => select.ToActorResponse());

            return View(ActorsResponse);
        }
    }
}
