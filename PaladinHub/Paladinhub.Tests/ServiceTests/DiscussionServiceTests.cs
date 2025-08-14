//using NUnit.Framework;
//using PaladinHub.Tests.Testing;
//using PaladinHub.Services.Discussions;
//using PaladinHub.Models.Discussions;
//using Microsoft.EntityFrameworkCore;

//namespace PaladinHub.Tests.ServiceTests;

//[TestFixture]
//public class DiscussionServiceTests
//{
//	[Test]
//	public async Task CreateAsync_Adds_Post()
//	{
//		using var db = DbContextFactory.CreateInMemory();
//		await SeedData.SeedDiscussionsAsync(db);

//		var service = new DiscussionService(db);
//		await service.CreateAsync(
//			userId: db.Users.First().Id,
//			new CreatePostViewModel { Title = "New title", Content = "Some content" });

//		Assert.That(await db.DiscussionPosts.AnyAsync(p => p.Title == "New title"), Is.True);
//	}

//	[Test]
//	public async Task GetByIdAsync_Returns_Null_For_Unknown()
//	{
//		using var db = DbContextFactory.CreateInMemory();
//		var service = new DiscussionService(db);
//		var result = await service.GetByIdAsync(Guid.NewGuid());
//		Assert.That(result, Is.Null);
//	}
//}
