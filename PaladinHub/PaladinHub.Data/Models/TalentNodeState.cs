using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Models
{
	public class TalentNodeState
	{
		[Key] public int Id { get; set; }

		[Required, MaxLength(100)]
		public string TreeKey { get; set; } = string.Empty;

		[Required, MaxLength(100)]
		public string NodeId { get; set; } = string.Empty;

		[Required]
		public bool IsActive { get; set; } = true;
	}
}
