using System.Collections.Generic;
using PaladinProject.Models;

namespace PaladinProject.Services.IService
{
	public interface ISpellbookService
	{
		List<Spell> GetAllSpells();
	}
}
