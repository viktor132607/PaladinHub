//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using PaladinHub.Controllers;
//using PaladinHub.Data.Entities;
//using PaladinHub.Services.Discussions;
//using PaladinHub.Services.Users;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using PaladinHub.Services.IService; // Добавено

//public class DiscussionControllerTests
//{
//	private readonly Mock<IDiscussionService> _mockDiscussionService;
//	private readonly Mock<IUserService> _mockUserService;
//	private readonly DiscussionController _controller;

//	public DiscussionControllerTests()
//	{
//		_mockDiscussionService = new Mock<IDiscussionService>();
//		_mockUserService = new Mock<IUserService>();
//		_controller = new DiscussionController(_mockDiscussionService.Object, _mockUserService.Object);
//	}

//	[Fact]
//	public async Task Index_ReturnsViewResult_WithListOfDiscussions()
//	{
//		// Arrange
//		var mockDiscussions = new List<Discussion>
//		{
//			new Discussion { Title = "Test Discussion 1" },
//			new Discussion { Title = "Test Discussion 2" }
//		};
//		_mockDiscussionService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockDiscussions);

//		// Act
//		var result = await _controller.Index();

//		// Assert
//		var viewResult = Assert.IsType<ViewResult>(result);
//		var model = Assert.IsAssignableFrom<List<Discussion>>(viewResult.ViewData.Model);
//		Assert.Equal(2, model.Count);
//	}

//	[Fact]
//	public async Task Details_ReturnsNotFound_WhenDiscussionDoesNotExist()
//	{
//		// Arrange
//		_mockDiscussionService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Discussion)null);

//		// Act
//		var result = await _controller.Details(Guid.NewGuid());

//		// Assert
//		Assert.IsType<NotFoundResult>(result);
//	}

//	[Fact]
//	public async Task Details_ReturnsViewResult_WhenDiscussionExists()
//	{
//		// Arrange
//		var mockId = Guid.NewGuid();
//		var mockDiscussion = new Discussion { Id = mockId, Title = "Test Discussion" };
//		_mockDiscussionService.Setup(s => s.GetByIdAsync(mockId)).ReturnsAsync(mockDiscussion);

//		// Act
//		var result = await _controller.Details(mockId);

//		// Assert
//		var viewResult = Assert.IsType<ViewResult>(result);
//		var model = Assert.IsAssignableFrom<Discussion>(viewResult.ViewData.Model);
//		Assert.Equal(mockId, model.Id);
//	}
//}