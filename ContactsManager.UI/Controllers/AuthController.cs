using Contact_Manager.Controllers;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]")]
    public class AuthController(
        UserManager<ApplicationUser> _userManager, 
        ILogger<AuthController> logger,
        SignInManager<ApplicationUser> _signInManager) : Controller
    {
        [Route("[action]")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(temp => temp.ErrorMessage));
                logger.LogError(errors.ToString());
                return View(registerDTO);
            }

            ApplicationUser user = new (){
                PersonName = registerDTO.PersonName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,

            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if(result.Succeeded)
            {
                //Sign In
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Persons");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                    logger.LogError(error.Description);
				}
			}

            return View(registerDTO);

        }


        [Route("[action]")]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            return View();
        }
    }
}
