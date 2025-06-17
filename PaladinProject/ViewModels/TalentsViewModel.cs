using PaladinProject.Helpers;
using System.Collections.Generic;
using TalentBuilder.Models;

namespace PaladinProject.ViewModels
{
	public class TalentsViewModel
	{

		public string Description { get; set; }
		public List<string> SpellNames { get; set; }
		public SpellBook SpellBook { get; set; }
	}
}
