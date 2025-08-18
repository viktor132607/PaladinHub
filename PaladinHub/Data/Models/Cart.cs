using PaladinHub.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Models
{
	public class Cart
	{
		[Key] 
		public Guid Id { get; init; } = Guid.NewGuid();

		public bool IsArchived { get; set; }

		public string? OrderDate { get; set; }

		[Required]
		public string UserId { get; set; } = default!;

		public User User { get; set; } = default!;

		public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

		public ICollection<Product> Products { get; set; } = new List<Product>();

		public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
	}
}
