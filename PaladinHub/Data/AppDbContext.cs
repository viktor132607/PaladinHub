using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;

namespace PaladinHub.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Spell> Spellbook { get; set; }
		public DbSet<Spell> Spells { get; set; }
		public DbSet<User> User { get; set; }

		public DbSet<Product> Products { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartProduct> CartProduct { get; set; }

		public DbSet<DiscussionPost> DiscussionPosts { get; set; }
		public DbSet<DiscussionComment> DiscussionComments { get; set; }
		public DbSet<DiscussionLike> DiscussionLikes { get; set; }
		public DbSet<DiscussionCommentLike> DiscussionCommentLikes { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// --- Carts ---
			builder.Entity<Cart>().HasMany(c => c.Products);
			builder.Entity<CartProduct>().HasKey(x => new { x.ProductId, x.CartId });

			builder.Entity<User>()
				.HasOne(u => u.Cart)
				.WithOne(c => c.User)
				.HasForeignKey<Cart>(c => c.UserId);

			builder.Entity<Cart>()
				.HasOne(c => c.User)
				.WithOne(u => u.Cart)
				.HasForeignKey<User>(u => u.CartId);

			// --- Discussion Comments ---
			builder.Entity<DiscussionComment>()
				.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.HasForeignKey(c => c.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			// --- Discussion Likes (posts) ---
			builder.Entity<DiscussionLike>()
				.HasOne(l => l.Post)
				.WithMany(p => p.LikesCollection)
				.HasForeignKey(l => l.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<DiscussionLike>()
				.HasOne(l => l.User)
				.WithMany()
				.HasForeignKey(l => l.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<DiscussionLike>()
				.HasIndex(l => new { l.PostId, l.UserId })
				.IsUnique(); // един лайк на потребител за пост

			// --- Discussion Comment Likes ---
			builder.Entity<DiscussionCommentLike>()
				.HasOne(l => l.Comment)
				.WithMany(c => c.LikesCollection)
				.HasForeignKey(l => l.CommentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<DiscussionCommentLike>()
				.HasOne(l => l.User)
				.WithMany()
				.HasForeignKey(l => l.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<DiscussionCommentLike>()
				.HasIndex(l => new { l.CommentId, l.UserId })
				.IsUnique(); // един лайк на потребител за коментар
		}
	}
}
