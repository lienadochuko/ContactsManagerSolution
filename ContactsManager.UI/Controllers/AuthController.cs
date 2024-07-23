using Contact_Manager.Controllers;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
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
		RoleManager<ApplicationRole> _roleManager,
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
				//Check Status of Radio Button
				if(registerDTO.UserType == UserTypeOptions.Developer)
				{
					//Create 'Developer' Role
					//Add the new user into 'Admin' role
					if (await _roleManager.FindByNameAsync(UserTypeOptions.Developer.ToString()) is null)
					{
						ApplicationRole applicationRole = new ApplicationRole()
						{
							Name = UserTypeOptions.Developer.ToString()
                        };
						await _roleManager.CreateAsync(applicationRole);
					}
                    //Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Developer.ToString());
                }
                else if (registerDTO.UserType == UserTypeOptions.Admin)
				{
                    //Create 'Admin' Role
                    //Add the new user into 'Developer' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.Admin.ToString()
                        };
                        await _roleManager.CreateAsync(applicationRole);
                    }
                    //Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else if (registerDTO.UserType == UserTypeOptions.User)
                {
                    //Create 'User' Role
                    //Add the new user into 'User' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.User.ToString()
                        };
                        await _roleManager.CreateAsync(applicationRole);
                    }
                    //Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }
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
        [Authorize(Policy = "NotAuthenticated")]
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
				//admin
				ApplicationUser applicationUser = await _userManager.FindByEmailAsync(loginDTO.Email);
				if (applicationUser != null)
				{
					if (await _userManager.IsInRoleAsync(applicationUser, UserTypeOptions.Admin.ToString()))
					{
						return RedirectToAction("Index", "Home", new { area = "Admin" });
					}
				}
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

		[Route("[action]")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(PersonsController.Index), "Persons");
		}

		[Route("[action]")]
		public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
	{
			ApplicationUser user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return Json(true);
			}
			else
			{
				return Json(false);
			}
		}
	}
}
