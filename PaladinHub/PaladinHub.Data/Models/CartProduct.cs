﻿using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Models
{
	public class CartProduct
	{
		[Required]
		public Guid CartId { get; init; }   

		public virtual Cart Cart { get; set; } = default!;

		[Required]
		public string ProductId { get; init; } = default!; 

		public virtual Product Product { get; set; } = default!;

		[Required]
		public int Quantity { get; set; }
	}
}
