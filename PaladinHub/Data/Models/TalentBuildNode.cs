using System.ComponentModel.DataAnnotations;

namespace PaladinHub.Data.Models
{
	public class TalentBuildNode
	{
		[Key] public int Id { get; set; }

		[Required] public int BuildId { get; set; }

		[Required, MaxLength(100)]
		public string NodeId { get; set; } = string.Empty;

		public bool IsActive { get; set; }

		public TalentBuild? Build { get; set; }
	}
}
