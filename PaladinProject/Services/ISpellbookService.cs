using System.Collections.Generic;
using PaladinProject.Models;

namespace PaladinProject.Services
{
	public interface ISpellbookService
	{
		List<Spell> GetAllSpells();
	}
}
