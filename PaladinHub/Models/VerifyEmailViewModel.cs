﻿using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Models
{
	public class VerifyEmailViewModel
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress]
		public string Email { get; set; }
	}
}
