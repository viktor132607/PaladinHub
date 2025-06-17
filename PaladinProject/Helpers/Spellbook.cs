namespace PaladinProject.Helpers
{
	public class SpellBook
	{
		public string HighlightSpells(string input, IEnumerable<string> spellNames)
		{
			foreach (var spell in spellNames)
			{
				input = input.Replace(spell, $"<span style='color: yellow; font-weight: bold;'>{spell}</span>");
			}

			return input;
		}
	}
}
