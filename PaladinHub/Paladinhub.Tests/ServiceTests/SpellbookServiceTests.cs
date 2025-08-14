using Microsoft.EntityFrameworkCore;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public class SpellbookServiceTests
{
	private AppDbContext CreateDbContext()
	{
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;
		return new AppDbContext(options);
	}

	private async Task SeedDatabase(AppDbContext context)
	{
		var spells = new List<Spell>
		{
			new Spell { Id = 1, Name = "Holy Shock", Description = "A jolt of Holy energy.", Icon = "icon1" },
			new Spell { Id = 2, Name = "Crusader Strike", Description = "Strikes the target with Holy power.", Icon = "icon2" },
			new Spell { Id = 3, Name = "Avenging Wrath", Description = "Increases damage and healing.", Icon = "icon3" },
			new Spell { Id = 4, Name = "Hammer of Wrath", Description = "Hurls a divine hammer.", Icon = "icon4" }
		};
		await context.Spells.AddRangeAsync(spells);
		await context.SaveChangesAsync();
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnAllSpells_OrderedByName()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var service = new SpellbookService(context);

		// Act
		var result = await service.GetAllAsync();

		// Assert
		Assert.Equal(4, result.Count);
		Assert.Equal("Avenging Wrath", result[0].Name);
		Assert.Equal("Crusader Strike", result[1].Name);
		Assert.Equal("Hammer of Wrath", result[2].Name);
		Assert.Equal("Holy Shock", result[3].Name);
	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnCorrectSpell_WhenIdExists()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var service = new SpellbookService(context);

		// Act
		var result = await service.GetByIdAsync(2);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(2, result.Id);
		Assert.Equal("Crusader Strike", result.Name);
	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var service = new SpellbookService(context);

		// Act
		var result = await service.GetByIdAsync(999);

		// Assert
		Assert.Null(result);
	}

	[Theory]
	[InlineData("Holy", 2)]
	[InlineData("Strikes", 1)]
	[InlineData("wrath", 2)]
	[InlineData("hammer", 1)]
	[InlineData("a jolt", 1)]
	public async Task SearchAsync_ShouldReturnMatchingSpells_WhenTermIsProvided(string term, int expectedCount)
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var service = new SpellbookService(context);

		// Act
		var result = await service.SearchAsync(term);

		// Assert
		Assert.Equal(expectedCount, result.Count);
		Assert.All(result, s => Assert.True(s.Name!.Contains(term, StringComparison.OrdinalIgnoreCase) || (s.Description ?? "").Contains(term, StringComparison.OrdinalIgnoreCase)));
	}

	[Fact]
	public async Task SearchAsync_ShouldReturnAllSpells_WhenTermIsWhitespace()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var service = new SpellbookService(context);

		// Act
		var result = await service.SearchAsync("   ");

		// Assert
		Assert.Equal(4, result.Count);
	}

	[Fact]
	public async Task GetPagedAsync_ShouldReturnCorrectPageAndTotalCount()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var service = new SpellbookService(context);
		var pageSize = 2;
		var page = 2;

		// Act
		var (items, total) = await service.GetPagedAsync(page, pageSize);

		// Assert
		Assert.Equal(4, total);
		Assert.Equal(2, items.Count);
		Assert.Equal("Hammer of Wrath", items[0].Name);
		Assert.Equal("Holy Shock", items[1].Name);
	}

	[Fact]
	public async Task GetPagedAsync_WithSearchTerm_ShouldReturnCorrectPagedResults()
	{
		// Arrange
		using var context = CreateDbContext();
		await SeedDatabase(context);
		var service = new SpellbookService(context);
		var pageSize = 1;
		var page = 2;
		var term = "wrath";

		// Act
		var (items, total) = await service.GetPagedAsync(page, pageSize, term);

		// Assert
		Assert.Equal(2, total);
		Assert.Single(items);
		Assert.Equal("Hammer of Wrath", items[0].Name);
	}
}