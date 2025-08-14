using PaladinHub.Services.SectionServices;
using PaladinHub.Models;
using Xunit;
using System.Linq;
using System.Collections.Generic;

public class HolySectionServiceTests
{
	private readonly HolySectionService _service;

	public HolySectionServiceTests()
	{
		_service = new HolySectionService();
	}

	[Fact]
	public void ControllerName_ShouldReturnHoly()
	{
		Assert.Equal("Holy", _service.ControllerName);
	}

	[Fact]
	public void GetCoverImage_ShouldReturnCorrectUrl()
	{
		Assert.Equal("/images/TheHolyCover2.jpg", _service.GetCoverImage());
	}

	[Theory]
	[InlineData("Overview", "Holy Paladin Healer Guide - The War Within")]
	[InlineData("Talents", "Best Holy Paladin Talent Tree Builds - The War Within")]
	[InlineData("Rotation", "Holy Paladin Rotation Guide - The War Within")]
	public void GetPageTitle_ShouldReturnCorrectTitle(string actionName, string expectedTitle)
	{
		Assert.Equal(expectedTitle, _service.GetPageTitle(actionName));
	}

	[Theory]
	[InlineData("Overview")]
	[InlineData("Talents")]
	[InlineData("Gear")]
	public void GetPageText_ShouldReturnCorrectText(string actionName)
	{
		var text = _service.GetPageText(actionName);
		Assert.False(string.IsNullOrWhiteSpace(text));
	}

	[Fact]
	public void GetCurrentSectionButtons_ForConsumables_ShouldReturnCorrectButtons()
	{
		// Act
		var result = _service.GetCurrentSectionButtons("Consumables");

		// Assert
		Assert.NotNull(result);
		Assert.Equal(4, result.Count);
		Assert.True(result.All(b => b.IsAnchor));
		Assert.Contains(result, b => b.Text == "Enchants");
	}

	[Fact]
	public void GetOtherSectionButtons_ShouldReturnNonAnchorButtons()
	{
		// Act
		var result = _service.GetOtherSectionButtons();

		// Assert
		Assert.NotNull(result);
		Assert.Equal(8, result.Count);
		Assert.All(result, b => Assert.False(b.IsAnchor));
	}
}

public class ProtectionSectionServiceTests
{
	private readonly ProtectionSectionService _service;

	public ProtectionSectionServiceTests()
	{
		_service = new ProtectionSectionService();
	}

	[Fact]
	public void ControllerName_ShouldReturnProtection()
	{
		Assert.Equal("Protection", _service.ControllerName);
	}

	[Fact]
	public void GetCoverImage_ShouldReturnCorrectUrl()
	{
		Assert.Equal("/images/ProtCoverV5.png", _service.GetCoverImage());
	}

	[Theory]
	[InlineData("Overview", "Protection Paladin Tank Guide - The War Within")]
	[InlineData("Talents", "Best Protection Paladin Talent Tree Builds - The War Within")]
	[InlineData("Rotation", "Protection Paladin Rotation Guide - The War Within 11.1.7")]
	public void GetPageTitle_ShouldReturnCorrectTitle(string actionName, string expectedTitle)
	{
		Assert.Equal(expectedTitle, _service.GetPageTitle(actionName));
	}

	[Theory]
	[InlineData("Overview")]
	[InlineData("Talents")]
	[InlineData("Gear")]
	public void GetPageText_ShouldReturnCorrectText(string actionName)
	{
		var text = _service.GetPageText(actionName);
		Assert.False(string.IsNullOrWhiteSpace(text));
	}

	[Fact]
	public void GetCurrentSectionButtons_ForGear_ShouldReturnCorrectButtons()
	{
		// Act
		var result = _service.GetCurrentSectionButtons("Gear");

		// Assert
		Assert.NotNull(result);
		Assert.Equal(8, result.Count);
		Assert.True(result.All(b => b.IsAnchor));
		Assert.Contains(result, b => b.Text == "Overall BiS");
	}

	[Fact]
	public void GetOtherSectionButtons_ShouldReturnNonAnchorButtons()
	{
		// Act
		var result = _service.GetOtherSectionButtons();

		// Assert
		Assert.NotNull(result);
		Assert.Equal(8, result.Count);
		Assert.All(result, b => Assert.False(b.IsAnchor));
	}
}

public class RetributionSectionServiceTests
{
	private readonly RetributionSectionService _service;

	public RetributionSectionServiceTests()
	{
		_service = new RetributionSectionService();
	}

	[Fact]
	public void ControllerName_ShouldReturnRetribution()
	{
		Assert.Equal("Retribution", _service.ControllerName);
	}

	[Fact]
	public void GetCoverImage_ShouldReturnCorrectUrl()
	{
		Assert.Equal("/images/RetributionCoverOrig.jpg", _service.GetCoverImage());
	}

	[Theory]
	[InlineData("Overview", "Retribution Paladin DPS Guide - The War Within")]
	[InlineData("Talents", "Best Retribution Paladin Talent Tree Builds - The War Within")]
	[InlineData("Rotation", "Retribution Paladin Rotation Guide - The War Within")]
	public void GetPageTitle_ShouldReturnCorrectTitle(string actionName, string expectedTitle)
	{
		Assert.Equal(expectedTitle, _service.GetPageTitle(actionName));
	}

	[Theory]
	[InlineData("Overview")]
	[InlineData("Talents")]
	[InlineData("Gear")]
	public void GetPageText_ShouldReturnCorrectText(string actionName)
	{
		var text = _service.GetPageText(actionName);
		Assert.False(string.IsNullOrWhiteSpace(text));
	}

	[Fact]
	public void GetCurrentSectionButtons_ForRotation_ShouldReturnCorrectButtons()
	{
		// Act
		var result = _service.GetCurrentSectionButtons("Rotation");

		// Assert
		Assert.NotNull(result);
		Assert.Equal(5, result.Count);
		Assert.True(result.All(b => b.IsAnchor));
		Assert.Contains(result, b => b.Text == "How to Play");
	}

	[Fact]
	public void GetOtherSectionButtons_ShouldReturnNonAnchorButtons()
	{
		// Act
		var result = _service.GetOtherSectionButtons();

		// Assert
		Assert.NotNull(result);
		Assert.Equal(8, result.Count);
		Assert.All(result, b => Assert.False(b.IsAnchor));
	}
}