using PaladinHub.Data.Models;
using PaladinHub.Models.Discussions;

namespace PaladinHub.Services.Discussions
{
	public interface IDiscussionService
	{
		Task<IEnumerable<DiscussionPost>> GetAllAsync();
		Task<DiscussionPost?> GetByIdAsync(Guid id);
		Task<DiscussionComment?> GetCommentByIdAsync(Guid id);

		Task CreateAsync(string userId, CreatePostViewModel model);
		Task<bool> DeleteAsync(Guid id, string userId, bool isAdmin);

		Task<bool> ToggleLikeAsync(Guid postId, string userId);
		Task<bool> ToggleCommentLikeAsync(Guid commentId, string userId);

		Task<bool> AddCommentAsync(Guid postId, string userId, string content);
	}
}
