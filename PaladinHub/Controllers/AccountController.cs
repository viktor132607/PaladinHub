using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Data.Entities;
using PaladinHub.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> signInManager;
		private readonly UserManager<User> userManager;

		public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

				if (result.Succeeded)
				{
					return RedirectToAction("Home", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Email or password is incorrect.");
					return View(model);
				}
			}
			return View(model);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				User User = new User
				{
					FullName = model.Name,
					Email = model.Email,
					UserName = model.Email,
				};

				var result = await userManager.CreateAsync(User, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction("Login", "Account");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}

					return View(model);
				}
			}
			return View(model);
		}

		public IActionResult VerifyEmail()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.Email);

				if (user == null)
				{
					ModelState.AddModelError("", "Something is wrong!");
					return View(model);
				}
				else
				{
					return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
				}
			}
			return View(model);
		}

		public IActionResult ChangePassword(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				return RedirectToAction("VerifyEmail", "Account");
			}
			return View(new ChangePasswordViewModel { Email = username });
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.Email);
				if (user != null)
				{
					var result = await userManager.RemovePasswordAsync(user);
					if (result.Succeeded)
					{
						result = await userManager.AddPasswordAsync(user, model.NewPassword);
						return RedirectToAction("Login", "Account");
					}
					else
					{

						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}

						return View(model);
					}
				}
				else
				{
					ModelState.AddModelError("", "Email not found!");
					return View(model);
				}
			}
			else
			{
				ModelState.AddModelError("", "Something went wrong. try again.");
				return View(model);
			}
		}

		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Home", "Home");
		}
	}
}
