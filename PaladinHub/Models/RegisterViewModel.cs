using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required.")]
		[StringLength(40, MinimumLength = 8,
			ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Confirm Password is required.")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
