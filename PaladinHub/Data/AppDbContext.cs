using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;

namespace PaladinHub.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		// Entities
		public DbSet<Item> Items { get; set; }
		public DbSet<Spell> Spells { get; set; }
		public DbSet<User> User { get; set; }                 // оставяме, ако вече се ползва в проекта
		public DbSet<Product> Products { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartProduct> CartProduct { get; set; }
		public DbSet<DiscussionPost> DiscussionPosts { get; set; }
		public DbSet<DiscussionComment> DiscussionComments { get; set; }
		public DbSet<DiscussionLike> DiscussionLikes { get; set; }
		public DbSet<DiscussionCommentLike> DiscussionCommentLikes { get; set; }

		// Нови: Reviews + Gallery images
		public DbSet<ProductReview> ProductReviews { get; set; } = default!;
		public DbSet<ProductImage> ProductImages { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// ---------- Items ----------
			builder.Entity<Item>(e =>
			{
				e.ToTable("Items");
				e.HasKey(i => i.Id);
				e.Property(i => i.Name).IsRequired().HasMaxLength(100);
				e.Property(i => i.Icon).HasMaxLength(100);
				e.Property(i => i.SecondIcon).HasMaxLength(100);
				e.Property(i => i.Description).HasMaxLength(2000);
				e.Property(i => i.Url).HasMaxLength(300);
				e.Property(i => i.Quality).HasMaxLength(50);
				e.HasIndex(i => i.Name);
			});

			// ---------- Spells ----------
			builder.Entity<Spell>(e =>
			{
				e.ToTable("Spells");
				e.HasKey(s => s.Id);
				e.Property(s => s.Name).IsRequired().HasMaxLength(100);
				e.Property(s => s.Icon).HasMaxLength(100);
				e.Property(s => s.Description).HasMaxLength(2000);
				e.Property(s => s.Url).HasMaxLength(300);
				e.Property(s => s.Quality).IsRequired().HasMaxLength(50);
				e.HasIndex(s => s.Name);
			});

			// ---------- Carts ----------
			builder.Entity<Cart>().HasMany(c => c.Products);
			builder.Entity<CartProduct>().HasKey(x => new { x.ProductId, x.CartId });

			// 1:1 User <-> Cart (и двете страни имат FK полета според твоите модели)
			builder.Entity<User>()
				.HasOne(u => u.Cart)
				.WithOne(c => c.User)
				.HasForeignKey<Cart>(c => c.UserId);

			builder.Entity<Cart>()
				.HasOne(c => c.User)
				.WithOne(u => u.Cart)
				.HasForeignKey<User>(u => u.CartId);

			// ---------- Discussions ----------
			builder.Entity<DiscussionComment>()
				.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.HasForeignKey(c => c.PostId)
				.OnDelete(DeleteBehavior.Cascade);

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
				.IsUnique();

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
				.IsUnique();

			// ---------- Products ----------
			builder.Entity<Product>().HasIndex(p => p.Category);
			builder.Entity<Product>().HasIndex(p => p.Name);

			// ---------- ProductReviews ----------
			builder.Entity<ProductReview>(e =>
			{
				e.ToTable("ProductReviews");
				e.HasKey(r => r.Id);
				e.Property(r => r.Content).IsRequired().HasMaxLength(2000);
				e.Property(r => r.Rating).IsRequired();

				// по 1 ревю на потребител за даден продукт
				e.HasIndex(r => new { r.ProductId, r.UserId }).IsUnique();

				// FK към Product (каскадно триене на ревютата при триене на продукта)
				e.HasOne<Product>()
				 .WithMany()
				 .HasForeignKey(r => r.ProductId)
				 .OnDelete(DeleteBehavior.Cascade);
			});

			// ---------- ProductImages (галерия) ----------
			builder.Entity<ProductImage>(e =>
			{
				e.ToTable("ProductImages");
				e.HasKey(i => i.Id);
				e.Property(i => i.Url).IsRequired();

				// индекси за бързо взимане и правилна подредба
				e.HasIndex(i => new { i.ProductId, i.SortOrder });

				// FK към Product (каскадно)
				e.HasOne<Product>()
				 .WithMany()
				 .HasForeignKey(i => i.ProductId)
				 .OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
