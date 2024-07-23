using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.AdminArea.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, Developer")]
    public class HomeController : Controller
    {
        [Route("admin/home/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
