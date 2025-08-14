//using NUnit.Framework;
//using PaladinHub.Tests.Testing;

//namespace PaladinHub.Tests.ServiceTests;

//[TestFixture]
//public class ItemsServiceTests
//{
//	[Test]
//	public async Task GetPagedAsync_Filters_And_Paginates()
//	{
//		using var db = DbContextFactory.CreateInMemory();
//		await SeedData.SeedItemsAsync(db);
//		var service = new ItemsService(db);

//		var (items, total) = await service.GetPagedAsync(page: 1, pageSize: 2, term: "of");

//		Assert.That(items.Count, Is.EqualTo(2));
//		Assert.That(total, Is.EqualTo(3));
//	}

//	[Test]
//	public async Task SearchAsync_By_Term_Works()
//	{
//		using var db = DbContextFactory.CreateInMemory();
//		await SeedData.SeedItemsAsync(db);
//		var service = new ItemsService(db);

//		var list = await service.SearchAsync("Hammer");
//		Assert.That(list.Count, Is.EqualTo(1));
//		Assert.That(list[0].Name, Does.Contain("Hammer"));
//	}
//}
