using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;

namespace PaladinHub.Tests.Testing;

public static class SeedData
{
	public static async Task SeedDiscussionsAsync(AppDbContext db)
	{
		var user = new User { Id = Guid.NewGuid().ToString(), UserName = "tester" };
		db.Users.Add(user);
		db.DiscussionPosts.Add(new DiscussionPost { Id = Guid.NewGuid(), Title = "Hello", Content = "World", AuthorId = user.Id });
		db.DiscussionPosts.Add(new DiscussionPost { Id = Guid.NewGuid(), Title = "Second", Content = "Post", AuthorId = user.Id });
		await db.SaveChangesAsync();
	}

	public static async Task SeedItemsAsync(AppDbContext db)
	{
		db.Items.Add(new Item { Id = 1, Name = "Sword of Dawn", Quality = "Epic" });
		db.Items.Add(new Item { Id = 2, Name = "Shield of Night", Quality = "Rare" });
		db.Items.Add(new Item { Id = 3, Name = "Holy Hammer", Quality = "Epic" });
		await db.SaveChangesAsync();
	}
}
