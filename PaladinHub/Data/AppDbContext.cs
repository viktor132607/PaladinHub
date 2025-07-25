using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaladinHub.Data.Entities;

namespace PaladinHub.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Spell> Spellbook { get; set; }

		public DbSet<Spell> Spells { get; set; }
	}

}