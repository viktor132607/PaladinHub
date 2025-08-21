using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;

namespace PaladinHub.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		// ---------- Entities ----------
		public DbSet<Item> Items { get; set; } = default!;
		public DbSet<Spell> Spells { get; set; } = default!;
		public DbSet<User> User { get; set; } = default!;
		public DbSet<Product> Products { get; set; } = default!;
		public DbSet<Cart> Carts { get; set; } = default!;
		public DbSet<CartProduct> CartProduct { get; set; } = default!;
		public DbSet<DiscussionPost> DiscussionPosts { get; set; } = default!;
		public DbSet<DiscussionComment> DiscussionComments { get; set; } = default!;
		public DbSet<DiscussionLike> DiscussionLikes { get; set; } = default!;
		public DbSet<DiscussionCommentLike> DiscussionCommentLikes { get; set; } = default!;

		// Reviews + Gallery images
		public DbSet<ProductReview> ProductReviews { get; set; } = default!;
		public DbSet<ProductImage> ProductImages { get; set; } = default!;

		// Talent states / builds
		public DbSet<TalentNodeState> TalentNodeStates { get; set; } = default!;
		public DbSet<TalentBuild> TalentBuilds { get; set; } = default!;
		public DbSet<TalentBuildNode> TalentBuildNodes { get; set; } = default!;

		// Page Builder
		public DbSet<ContentPage> ContentPages { get; set; } = default!;
		public DbSet<DataPreset> DataPresets { get; set; } = default!;

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

			// 1:1 User <-> Cart
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
			builder.Entity<Product>(e =>
			{
				e.ToTable("Products");
				e.HasKey(p => p.Id);

				e.Property(p => p.Id).IsRequired(); // string GUID
				e.Property(p => p.Name).IsRequired().HasMaxLength(100);
				e.Property(p => p.Price).HasColumnType("decimal(18,2)");
				e.Property(p => p.Category).HasMaxLength(50);
				e.Property(p => p.Description).HasMaxLength(1000);

				e.HasIndex(p => p.Category);
				e.HasIndex(p => p.Name);

				// 1:0..1 Product -> ThumbnailImage
				e.HasOne(p => p.ThumbnailImage)
					.WithMany()
					.HasForeignKey(p => p.ThumbnailImageId)
					.OnDelete(DeleteBehavior.SetNull);

				e.HasIndex(p => p.ThumbnailImageId)
					.HasDatabaseName("IX_Products_ThumbnailImageId");
			});

			// ---------- ProductImages ----------
			builder.Entity<ProductImage>(e =>
			{
				e.ToTable("ProductImages");
				e.HasKey(i => i.Id);

				e.Property(i => i.ProductId).IsRequired();
				e.Property(i => i.Url).IsRequired().HasMaxLength(2048);
				e.Property(i => i.SortOrder);
				e.Property(i => i.AltText).HasMaxLength(300);
				e.Property(i => i.CreatedAt);

				e.HasIndex(i => new { i.ProductId, i.SortOrder })
					.IsUnique()
					.HasDatabaseName("UX_ProductImages_Product_SortOrder");

				e.HasOne(i => i.Product)
					.WithMany(p => p.Images)
					.HasForeignKey(i => i.ProductId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// ---------- ProductReviews ----------
			builder.Entity<ProductReview>(e =>
			{
				e.ToTable("ProductReviews");
				e.HasKey(r => r.Id);

				e.Property(r => r.Content).IsRequired().HasMaxLength(2000);
				e.Property(r => r.Rating).IsRequired();

				e.HasIndex(r => new { r.ProductId, r.UserId }).IsUnique();

				e.HasOne(r => r.Product)
					.WithMany(p => p.Reviews)
					.HasForeignKey(r => r.ProductId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// ---------- TalentNodeStates ----------
			builder.Entity<TalentNodeState>(e =>
			{
				e.ToTable("TalentNodeStates");
				e.HasKey(x => x.Id);

				e.Property(x => x.TreeKey).IsRequired().HasMaxLength(100);
				e.Property(x => x.NodeId).IsRequired().HasMaxLength(100);
				e.Property(x => x.IsActive).IsRequired();

				e.HasIndex(x => new { x.TreeKey, x.NodeId }).IsUnique();
			});

			// ---------- TalentBuilds ----------
			builder.Entity<TalentBuild>(e =>
			{
				e.ToTable("TalentBuilds");
				e.HasKey(x => x.Id);
				e.Property(x => x.TreeKey).IsRequired().HasMaxLength(100);
				e.Property(x => x.Name).IsRequired().HasMaxLength(100);
				e.Property(x => x.IsDefault).IsRequired();
				e.HasIndex(x => new { x.TreeKey, x.Name }).IsUnique();
			});

			// ---------- TalentBuildNodes ----------
			builder.Entity<TalentBuildNode>(e =>
			{
				e.ToTable("TalentBuildNodes");
				e.HasKey(x => x.Id);
				e.Property(x => x.NodeId).IsRequired().HasMaxLength(100);
				e.HasIndex(x => new { x.BuildId, x.NodeId }).IsUnique();

				e.HasOne(x => x.Build)
				 .WithMany(b => b.Nodes)
				 .HasForeignKey(x => x.BuildId)
				 .OnDelete(DeleteBehavior.Cascade);
			});

			// ---------- ContentPages ----------
			builder.Entity<ContentPage>(e =>
			{
				e.ToTable("ContentPages");
				e.HasKey(x => x.Id);

				e.Property(x => x.Section).IsRequired().HasMaxLength(50);
				e.Property(x => x.Slug).IsRequired().HasMaxLength(100);
				e.Property(x => x.Title).IsRequired().HasMaxLength(200);
				e.Property(x => x.JsonLayout).IsRequired();

				// конфигурация за UpdatedBy (optional)
				e.Property(x => x.UpdatedBy).HasMaxLength(100);

				// RowVersion: EF да изпраща стойност, + DEFAULT в DB
				e.Property(x => x.RowVersion)
					.IsRequired()
					.IsConcurrencyToken()
					.ValueGeneratedNever()
					.HasColumnType("bytea")
					.HasDefaultValue(System.Array.Empty<byte>());

				e.HasIndex(x => new { x.Section, x.Slug }).IsUnique();
			});

			// ---------- DataPresets ----------
			builder.Entity<DataPreset>(e =>
			{
				e.ToTable("DataPresets");
				e.HasKey(x => x.Id);
				e.Property(x => x.Name).IsRequired().HasMaxLength(150);
				e.Property(x => x.Entity).IsRequired().HasMaxLength(50);
				e.Property(x => x.JsonQuery).IsRequired();
				e.HasIndex(x => new { x.Entity, x.Name });
			});
		}
	}
}
