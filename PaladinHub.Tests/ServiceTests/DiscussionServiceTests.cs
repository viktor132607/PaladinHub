using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

// Namespace-ове на твоя проект
using PaladinHub.Data;
using PaladinHub.Data.Models;
using PaladinHub.Services.Discussions;
using PaladinHub.Models.Discussions;
using PaladinHub.Data.Entities; // Добавен using за класа User

// Namespace на тестовия проект
namespace PaladinHub.Tests.Services
{
	public class DiscussionServiceTests
	{
		// Помощен метод за създаване на нова база данни за всеки тест
		private AppDbContext CreateDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			return new AppDbContext(options);
		}

		// Помощен метод за запълване на базата данни с примерни данни
		private void SeedDatabase(AppDbContext context)
		{
			// Сега използваме твоя клас User
			var user1 = new User { Id = "user-1", UserName = "testuser1" };
			var user2 = new User { Id = "user-2", UserName = "testuser2" };
			context.Users.AddRange(user1, user2);

			var post1 = new DiscussionPost { Id = new Guid("4b17e436-1936-418b-ae16-c361991a0397"), Title = "Post 1", Content = "Content 1", AuthorId = "user-1", CreatedOn = DateTime.UtcNow };
			var post2 = new DiscussionPost { Id = new Guid("119101d2-06b4-4861-a5f1-8f5b843d41f3"), Title = "Post 2", Content = "Content 2", AuthorId = "user-2", CreatedOn = DateTime.UtcNow.AddMinutes(1) };
			context.DiscussionPosts.AddRange(post1, post2);

			var comment1 = new DiscussionComment { Id = new Guid("9e083651-7f8e-486b-88e8-d7b003a3d5e2"), PostId = post1.Id, Content = "Comment 1", AuthorId = "user-2" };
			context.DiscussionComments.Add(comment1);

			context.SaveChanges();
		}

		[Fact]
		public async Task GetAllAsync_ShouldReturnAllPostsOrderedByDate()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);

			// Act
			var result = await service.GetAllAsync();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Equal("Post 2", result.First().Title);
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnPost_WhenPostExists()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = new Guid("4b17e436-1936-418b-ae16-c361991a0397");

			// Act
			var result = await service.GetByIdAsync(postId);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(postId, result.Id);
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnNull_WhenPostDoesNotExist()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = Guid.NewGuid();

			// Act
			var result = await service.GetByIdAsync(postId);

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public async Task CreateAsync_ShouldAddNewPostToDatabase()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var model = new CreatePostViewModel { Title = "New Post", Content = "Test Content" };

			// Act
			await service.CreateAsync("user-1", model);

			// Assert
			Assert.Equal(3, context.DiscussionPosts.Count());
			Assert.Contains(context.DiscussionPosts, p => p.Title == "New Post");
		}

		[Fact]
		public async Task DeleteAsync_ShouldRemovePost_WhenUserIsAuthor()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = new Guid("4b17e436-1936-418b-ae16-c361991a0397");

			// Act
			var result = await service.DeleteAsync(postId, "user-1", isAdmin: false);

			// Assert
			Assert.True(result);
			Assert.Equal(1, context.DiscussionPosts.Count());
		}

		[Fact]
		public async Task DeleteAsync_ShouldNotRemovePost_WhenUserIsNotAuthor()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = new Guid("4b17e436-1936-418b-ae16-c361991a0397");

			// Act
			var result = await service.DeleteAsync(postId, "user-2", isAdmin: false);

			// Assert
			Assert.False(result);
			Assert.Equal(2, context.DiscussionPosts.Count());
		}

		[Fact]
		public async Task ToggleLikeAsync_ShouldAddLike_WhenUserHasNotLiked()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = new Guid("4b17e436-1936-418b-ae16-c361991a0397");

			// Act
			var result = await service.ToggleLikeAsync(postId, "user-2");

			// Assert
			Assert.True(result);
			var post = context.DiscussionPosts.First(p => p.Id == postId);
			Assert.Equal(1, post.Likes);
			Assert.Equal(1, context.DiscussionLikes.Count());
		}

		[Fact]
		public async Task ToggleLikeAsync_ShouldRemoveLike_WhenUserHasLiked()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = new Guid("4b17e436-1936-418b-ae16-c361991a0397");
			var like = new DiscussionLike { PostId = postId, UserId = "user-2" };
			context.DiscussionLikes.Add(like);
			var post = context.DiscussionPosts.First(p => p.Id == postId);
			post.Likes++;
			await context.SaveChangesAsync();

			// Act
			var result = await service.ToggleLikeAsync(postId, "user-2");

			// Assert
			Assert.True(result);
			Assert.Equal(0, post.Likes);
			Assert.Empty(context.DiscussionLikes);
		}

		[Fact]
		public async Task AddCommentAsync_ShouldAddComment_WhenPostExists()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = new Guid("4b17e436-1936-418b-ae16-c361991a0397");

			// Act
			var result = await service.AddCommentAsync(postId, "user-2", "New comment content");

			// Assert
			Assert.True(result);
			Assert.Equal(2, context.DiscussionComments.Count());
			Assert.Contains(context.DiscussionComments, c => c.Content == "New comment content");
		}

		[Fact]
		public async Task AddCommentAsync_ShouldReturnFalse_WhenPostDoesNotExist()
		{
			// Arrange
			using var context = CreateDbContext();
			SeedDatabase(context);
			var service = new DiscussionService(context);
			var postId = Guid.NewGuid();

			// Act
			var result = await service.AddCommentAsync(postId, "user-1", "Comment content");

			// Assert
			Assert.False(result);
			Assert.Equal(1, context.DiscussionComments.Count());
		}
	}
}