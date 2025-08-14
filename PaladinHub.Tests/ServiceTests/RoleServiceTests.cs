using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PaladinHub.Data;
using PaladinHub.Data.Entities;
using PaladinHub.Services.Roles;
using System.Threading.Tasks;
using Xunit;

public class RoleServiceTests
{
	private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
	private readonly Mock<UserManager<User>> _mockUserManager;
	private readonly RoleService _roleService;

	public RoleServiceTests()
	{
		// Mocking RoleManager
		_mockRoleManager = new Mock<RoleManager<IdentityRole>>(
			Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

		// Mocking UserManager
		var userStoreMock = new Mock<IUserStore<User>>();
		_mockUserManager = new Mock<UserManager<User>>(
			userStoreMock.Object, null, null, null, null, null, null, null, null);

		_roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);
	}

	[Fact]
	public async Task CreateRole_ShouldReturnTrue_WhenRoleIsCreatedSuccessfully()
	{
		// Arrange
		var roleName = "TestRole";
		_mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
			.ReturnsAsync(IdentityResult.Success);

		// Act
		var result = await _roleService.CreateRole(roleName);

		// Assert
		Assert.True(result);
		_mockRoleManager.Verify(rm => rm.CreateAsync(It.Is<IdentityRole>(r => r.Name == roleName)), Times.Once);
	}

	[Fact]
	public async Task CreateRole_ShouldReturnFalse_WhenRoleNameIsNullOrEmpty()
	{
		// Arrange
		string roleName = null;

		// Act
		var result = await _roleService.CreateRole(roleName);

		// Assert
		Assert.False(result);
		_mockRoleManager.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
	}

	[Fact]
	public async Task AddUserToRole_ShouldReturnTrue_WhenUserIsAddedSuccessfully()
	{
		// Arrange
		var user = new User { UserName = "testuser" };
		var roleName = "TestRole";
		_mockUserManager.Setup(um => um.AddToRoleAsync(user, roleName))
			.ReturnsAsync(IdentityResult.Success);

		// Act
		var result = await _roleService.AddUserToRole(user, roleName);

		// Assert
		Assert.True(result);
		_mockUserManager.Verify(um => um.AddToRoleAsync(user, roleName), Times.Once);
	}

	[Fact]
	public async Task AddUserToRole_ShouldReturnFalse_WhenRoleNameIsNullOrEmpty()
	{
		// Arrange
		var user = new User { UserName = "testuser" };
		string roleName = null;

		// Act
		var result = await _roleService.AddUserToRole(user, roleName);

		// Assert
		Assert.False(result);
		_mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
	}

	[Fact]
	public async Task AddUserToRole_ShouldReturnFalse_WhenUserIsNull()
	{
		// Arrange
		User user = null;
		var roleName = "TestRole";

		// Act
		var result = await _roleService.AddUserToRole(user, roleName);

		// Assert
		Assert.False(result);
		_mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
	}
}