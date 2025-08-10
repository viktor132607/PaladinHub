namespace PaladinHub.Models
{
	public sealed class CartLine
	{
		public Guid ProductId { get; set; } 
		public int Quantity { get; set; }
	}
}
