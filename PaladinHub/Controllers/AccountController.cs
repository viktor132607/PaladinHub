using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Data.Entities;
using PaladinHub.Models;

namespace UserApp.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;

		public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Login() => View();

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
			{
				ModelState.AddModelError(string.Empty, "Email and Password are required.");
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(
				userName: model.Email,
				password: model.Password,
				isPersistent: model.RememberMe,
				lockoutOnFailure: false);

			if (result.Succeeded)
				return RedirectToAction("Home", "Home");

			ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
			return View(model);
		}

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			if (string.IsNullOrWhiteSpace(model.Email) ||
				string.IsNullOrWhiteSpace(model.Name) ||
				string.IsNullOrWhiteSpace(model.Password))
			{
				ModelState.AddModelError(string.Empty, "All fields are required.");
				return View(model);
			}

			var user = new User
			{
				FullName = model.Name,
				Email = model.Email,
				UserName = model.Email
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
				return RedirectToAction("Login", "Account");

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(model);
		}

		[HttpGet]
		public IActionResult VerifyEmail() => View();

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			if (string.IsNullOrWhiteSpace(model.Email))
			{
				ModelState.AddModelError(string.Empty, "Email is required.");
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Email not found.");
				return View(model);
			}

			return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
		}

		[HttpGet]
		public IActionResult ChangePassword(string? username)
		{
			if (string.IsNullOrWhiteSpace(username))
				return RedirectToAction("VerifyEmail", "Account");

			return View(new ChangePasswordViewModel { Email = username });
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.NewPassword))
			{
				ModelState.AddModelError(string.Empty, "Email and New Password are required.");
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Email not found.");
				return View(model);
			}

			var remove = await _userManager.RemovePasswordAsync(user);
			if (!remove.Succeeded)
			{
				foreach (var e in remove.Errors) ModelState.AddModelError(string.Empty, e.Description);
				return View(model);
			}

			var add = await _userManager.AddPasswordAsync(user, model.NewPassword);
			if (add.Succeeded) return RedirectToAction("Login", "Account");

			foreach (var e in add.Errors) ModelState.AddModelError(string.Empty, e.Description);
			return View(model);
		}

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Home", "Home");
		}
	}
}
