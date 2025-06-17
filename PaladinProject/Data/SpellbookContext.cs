using Microsoft.EntityFrameworkCore;
using paladinproject.models;

namespace PaladinProject.Data
{
	public class SpellbookContext : DbContext
	{

		public DbSet<Spell> Spellbook { get; set; }

		public SpellbookContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Spell>()
				.HasData(
				new Spell() { Id = 1, Name = "Crusader Strike" },
				new Spell() { Id = 2, Name = "Judgement" },
				new Spell() { Id = 3, Name = "Divine Shield" },
				new Spell() { Id = 4, Name = "Hand Of Freedom" },
				new Spell() { Id = 5, Name = "Cleanse" },
				new Spell() { Id = 6, Name = "Blessing Of Protection" },
				new Spell() { Id = 7, Name = "Divine Steed" },
				new Spell() { Id = 8, Name = "Shield Of Righterous" },
				new Spell() { Id = 9, Name = "Lay Of Hands" },
				new Spell() { Id = 10, Name = "Flash Of Light" }
				);

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Spell> Spells { get; set; }

	}
}