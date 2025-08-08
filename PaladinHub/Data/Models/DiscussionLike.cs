using PaladinHub.Data.Entities;

namespace PaladinHub.Data.Models
{
	public class DiscussionLike
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid PostId { get; set; }
		public DiscussionPost Post { get; set; } = default!;
		public string UserId { get; set; } = default!;
		public User User { get; set; } = default!;
	}
}
