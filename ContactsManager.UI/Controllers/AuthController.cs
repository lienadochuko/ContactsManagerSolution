using Contact_Manager.Controllers;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactsManager.UI.Controllers
{
	[Route("[controller]")]
	[AllowAnonymous]
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
			if (ModelState.IsValid == false)
			{
				var errors = ModelState.Values.SelectMany(x => x.Errors.Select(temp => temp.ErrorMessage));
				logger.LogError(errors.ToString());
				return View(registerDTO);
			}

			ApplicationUser user = new()
			{
				PersonName = registerDTO.PersonName,
				Email = registerDTO.Email,
				UserName = registerDTO.Email,
				PhoneNumber = registerDTO.PhoneNumber,

			};

			IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
			if (result.Succeeded)
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
		public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
		{
			if (ModelState.IsValid == false)
			{
				var errors = ModelState.Values.SelectMany(x => x.Errors.Select(temp => temp.ErrorMessage));
				logger.LogError(errors.ToString());
				return View(loginDTO);
			}
			var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: true);

			if (result.Succeeded)
			{
				if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
				{
					return LocalRedirect(ReturnUrl);
				}

				return RedirectToAction("Index", "Persons");
			}
			else
			{
				ModelState.AddModelError("Login", "Invalid email or password");
				return View(loginDTO);
			}


		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(PersonsController.Index), "Persons");
		}
	}
}
