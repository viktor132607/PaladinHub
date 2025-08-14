//using System.Collections.Generic;
//using System.Linq;
//using PaladinHub.Data.Entities;
//using PaladinHub.Models.Talents;
//using PaladinHub.Services.TalentTrees;
//using Xunit;

//public class TalentTreeBuildersTests
//{
//	private List<Spell> _mockSpells;

//	public TalentTreeBuildersTests()
//	{
//		_mockSpells = new List<Spell>
//		{
//			new Spell { Id = 1, Name = "Holy Shock" },
//			new Spell { Id = 2, Name = "Crusader Strike" },
//			new Spell { Id = 3, Name = "Avenging Wrath" },
//			new Spell { Id = 4, Name = "Hammer of Wrath" },
//			new Spell { Id = 5, Name = "Light of Dawn" },
//			new Spell { Id = 6, Name = "Judgment" },
//			new Spell { Id = 7, Name = "Shield of the Righteous" },
//			new Spell { Id = 8, Name = "Divine Shield" },
//			new Spell { Id = 9, Name = "Word of Glory" },
//			new Spell { Id = 10, Name = "Consecration" },
//		};
//	}

//	// --- HolySpecTreeBuilder Tests ---
//	[Fact]
//	public void HolySpecTreeBuilder_BuildTree_ShouldReturnCorrectTree()
//	{
//		// Arrange
//		var builder = new HolySpecTreeBuilder();

//		// Act
//		var tree = builder.BuildTree(_mockSpells);

//		// Assert
//		Assert.NotNull(tree);
//		Assert.Equal("Holy", tree.Name);
//		Assert.Equal("holy", tree.Key);
//		Assert.Equal(32, tree.Nodes.Count);
//	}

//	[Fact]
//	public void HolySpecTreeBuilder_BaseKey_ShouldReturnHoly()
//	{
//		// Arrange
//		var builder = new HolySpecTreeBuilder();

//		// Act & Assert
//		Assert.Equal("holy", builder.BaseKey);
//	}

//	// --- ProtectionSpecTreeBuilder Tests ---
//	[Fact]
//	public void ProtectionSpecTreeBuilder_BuildTree_ShouldReturnCorrectTree()
//	{
//		// Arrange
//		var builder = new ProtectionSpecTreeBuilder();

//		// Act
//		var tree = builder.BuildTree(_mockSpells);

//		// Assert
//		Assert.NotNull(tree);
//		Assert.Equal("Protection", tree.Name);
//		Assert.Equal("protection", tree.Key);
//		Assert.Equal(31, tree.Nodes.Count);
//	}

//	[Fact]
//	public void ProtectionSpecTreeBuilder_BaseKey_ShouldReturnProtection()
//	{
//		// Arrange
//		var builder = new ProtectionSpecTreeBuilder();

//		// Act & Assert
//		Assert.Equal("protection", builder.BaseKey);
//	}

//	// --- RetributionSpecTreeBuilder Tests ---
//	[Fact]
//	public void RetributionSpecTreeBuilder_BuildTree_ShouldReturnCorrectTree()
//	{
//		// Arrange
//		var builder = new RetributionSpecTreeBuilder();

//		// Act
//		var tree = builder.BuildTree(_mockSpells);

//		// Assert
//		Assert.NotNull(tree);
//		Assert.Equal("Retribution", tree.Name);
//		Assert.Equal("retribution", tree.Key);
//		Assert.Equal(32, tree.Nodes.Count);
//	}

//	[Fact]
//	public void RetributionSpecTreeBuilder_BaseKey_ShouldReturnRetribution()
//	{
//		// Arrange
//		var builder = new RetributionSpecTreeBuilder();

//		// Act & Assert
//		Assert.Equal("retribution", builder.BaseKey);
//	}

//	// --- PaladinClassTreeBuilder Tests ---
//	[Fact]
//	public void PaladinClassTreeBuilder_BuildTree_ShouldReturnCorrectTree()
//	{
//		// Arrange
//		var builder = new PaladinClassTreeBuilder();

//		// Act
//		var tree = builder.BuildTree(_mockSpells);

//		// Assert
//		Assert.NotNull(tree);
//		Assert.Equal("Paladin", tree.Name);
//		Assert.Equal("paladin", tree.Key);
//		Assert.Equal(32, tree.Nodes.Count);
//	}

//	[Fact]
//	public void PaladinClassTreeBuilder_BaseKey_ShouldReturnPaladin()
//	{
//		// Arrange
//		var builder = new PaladinClassTreeBuilder();

//		// Act & Assert
//		Assert.Equal("paladin", builder.BaseKey);
//	}
//}