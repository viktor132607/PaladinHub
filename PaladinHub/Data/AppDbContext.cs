using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Entities;
using PaladinHub.Data.Models;

namespace PaladinHub.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Spell> Spellbook { get; set; }

		public DbSet<Spell> Spells { get; set; }

		public DbSet<User> ApplicationUser { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Cart> Carts { get; set; }

		public DbSet<CartProduct> CartProduct { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<Cart>().HasMany(c => c.Products);
			builder.Entity<CartProduct>().HasKey(x => new { x.ProductId, x.CartId });
			builder.Entity<User>().HasOne(t => t.Cart)
					 .WithOne(t => t.User)
					 .HasForeignKey<Cart>(t => t.UserId);

			builder.Entity<Cart>().HasOne(t => t.User)
					 .WithOne(t => t.Cart)
					 .HasForeignKey<User>(t => t.CartId);
		}
	}

}