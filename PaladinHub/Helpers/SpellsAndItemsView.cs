using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using PaladinHub.Data.Entities;
using PaladinHub.Models.Talents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PaladinHub.Helpers
{
	public abstract class SpellsAndItemsView : RazorPage<PaladinHub.Models.CombinedViewModel>
	{
		protected static string NormalizeKey(string? raw)
		{
			if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
			var s = raw.Trim();
			s = s.Replace('’', '\'').Replace('“', '"').Replace('”', '"');
			s = Regex.Replace(s, @"\s+", " ");
			return s.ToUpperInvariant();
		}

		protected IReadOnlyDictionary<string, Spell> SpellsByName =>
			(Model?.Spells ?? Enumerable.Empty<Spell>())
				.Where(s => !string.IsNullOrWhiteSpace(s.Name))
				.GroupBy(s => NormalizeKey(s.Name))
				.Select(g => g.First())
				.ToDictionary(s => NormalizeKey(s.Name), s => s);

		protected IReadOnlyDictionary<string, Item> ItemsByName =>
			(Model?.Items ?? Enumerable.Empty<Item>())
				.Where(i => !string.IsNullOrWhiteSpace(i.Name))
				.GroupBy(i => NormalizeKey(i.Name))
				.Select(g => g.First())
				.ToDictionary(i => NormalizeKey(i.Name), i => i);

		private static string ResolveIconPath(string? icon, string folder, string fallback = "placeholder.png")
		{
			if (string.IsNullOrWhiteSpace(icon))
				return $"/images/{folder}/{fallback}";
			var s = icon.Trim().TrimStart('~');
			if (s.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
				s.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				return s;
			if (s.Contains('/'))
				return s.StartsWith("/") ? s : "/" + s;
			return $"/images/{folder}/{s}";
		}

		protected IHtmlContent SpellLink(string spellName, int size = 20)
		{
			var key = NormalizeKey(spellName);
			if (string.IsNullOrEmpty(key)) return new HtmlString("");

			if (!SpellsByName.TryGetValue(key, out var spell))
				return new HtmlString($"<span class='spell-fallback'>{spellName}</span>");

			var url = string.IsNullOrWhiteSpace(spell.Url) ? "#" : spell.Url!;
			var icon = ResolveIconPath(spell.Icon, "SpellIcons");

			var qualityClass = string.IsNullOrWhiteSpace(spell.Quality)
				? "spell"
				: spell.Quality.Trim().ToLowerInvariant();

			var safeName = System.Net.WebUtility.HtmlEncode(spell.Name);

			var html = $@"
<span class='spell-ref'>
  <a href='{url}' target='_blank' class='spell-link spell {qualityClass}' style='display:inline-flex;align-items:baseline;gap:6px;text-decoration:none;'>
    <img src='{icon}' alt='{safeName}' width='{size}' height='{size}' style='vertical-align:text-bottom;' />
    <span class='spell {qualityClass}'>{safeName}</span>
  </a>
</span>";
			return new HtmlString(html);
		}

		protected IHtmlContent ItemLink(string itemName, int size = 20)
		{
			var key = NormalizeKey(itemName);
			if (string.IsNullOrEmpty(key)) return new HtmlString("");

			if (!ItemsByName.TryGetValue(key, out var item))
				return new HtmlString($"<span class='item-fallback'>{itemName}</span>");

			var url = string.IsNullOrWhiteSpace(item.Url) ? "#" : item.Url!;
			var icon1 = ResolveIconPath(item.Icon, "itemIcons");
			var icon2 = ResolveIconPath(item.SecondIcon, "itemIcons", "");

			var qualityClass = string.IsNullOrWhiteSpace(item.Quality)
				? "common"
				: item.Quality.Trim().ToLowerInvariant();

			var safeName = System.Net.WebUtility.HtmlEncode(item.Name);

			var secondIcon = string.IsNullOrWhiteSpace(item.SecondIcon)
				? ""
				: $"<img src='{icon2}' title='{System.Net.WebUtility.HtmlEncode(item.SecondIcon)}' width='{size}' height='{size}' style='vertical-align:text-bottom;' />";

			var html = $@"
<span class='item-ref'>
  <a href='{url}' target='_blank' class='item-link item {qualityClass}' style='display:inline-flex;align-items:baseline;gap:6px;text-decoration:none;'>
    <img src='{icon1}' alt='{safeName}' width='{size}' height='{size}' style='vertical-align:text-bottom;' />
    <span class='item {qualityClass}'>{safeName}</span>
    {secondIcon}
  </a>
</span>";
			return new HtmlString(html);
		}

		protected IHtmlContent Item(string itemName, int size = 20) => ItemLink(itemName, size);
		protected IHtmlContent Spell(string spellName, int size = 20) => SpellLink(spellName, size);
		protected IHtmlContent item(string itemName, int size = 20) => ItemLink(itemName, size);
		protected IHtmlContent spell(string spellName, int size = 20) => SpellLink(spellName, size);

		protected IHtmlContent SpellNode(TalentNodeViewModel n)
		{
			var name = n.SpellName ?? string.Empty;
			Spell? spell = null;
			if (!string.IsNullOrWhiteSpace(name))
			{
				SpellsByName.TryGetValue(NormalizeKey(name), out spell);
				if (spell == null)
				{
					return new HtmlString(
						$"<div class='node node-fallback' style='grid-column:{n.Col};grid-row:{n.Row};' data-id='{n.Id}'>{name}</div>");
				}
			}

			var shapeClass = $"node-{(n.Shape ?? "circle").ToLowerInvariant()}";
			var stateClass = n.Active ? "active" : "inactive";
			var alt = spell?.Name ?? n.SpellName ?? n.Id;
			var icon = ResolveIconPath(spell?.Icon, "SpellIcons");
			var url = string.IsNullOrWhiteSpace(spell?.Url) ? "#" : spell!.Url!;
			var requires = n.Requires != null && n.Requires.Count > 0 ? string.Join(",", n.Requires) : string.Empty;

			var html = $@"
<div class='node {shapeClass} {stateClass}' style='grid-column:{n.Col};grid-row:{n.Row};'
     data-id='{n.Id}' data-cost='{n.Cost}' data-requires='{requires}'>
  <a href='{url}' target='_blank' tabindex='-1' aria-label='{alt}'>
    <img src='{icon}' alt='{alt}' />
  </a>
</div>";
			return new HtmlString(html);
		}

		protected IHtmlContent SpellNode(string spellName, int col, int row, string shape = "circle")
		{
			var key = NormalizeKey(spellName);
			if (!SpellsByName.TryGetValue(key, out var spell))
			{
				return new HtmlString(
					$"<div class='node node-fallback' style='grid-column:{col};grid-row:{row};'>{spellName}</div>");
			}

			var shapeClass = $"node-{(shape ?? "circle").ToLowerInvariant()}";
			var icon = ResolveIconPath(spell.Icon, "SpellIcons");
			var url = string.IsNullOrWhiteSpace(spell.Url) ? "#" : spell.Url!;
			var alt = spell.Name ?? spellName;

			var html = $@"
<div class='node {shapeClass} active' style='grid-column:{col};grid-row:{row};'>
  <a href='{url}' target='_blank' tabindex='-1' aria-label='{alt}'>
    <img src='{icon}' alt='{alt}' />
  </a>
</div>";
			return new HtmlString(html);
		}

		protected IHtmlContent Line(int fromCol, int fromRow, int toCol, int toRow)
		{
			var style = LineStyle(fromCol, fromRow, toCol, toRow, -8, -8);
			return new HtmlString($"<div class='line' style='{style}'></div>");
		}

		protected string LineStyle(int fromCol, int fromRow, int toCol, int toRow, double offsetX = -8, double offsetY = -8)
		{
			const int cellW = 50;
			const int cellH = 60;
			const int gap = 20;

			int stepX = cellW + gap;
			int stepY = cellH + gap;

			double x = (fromCol - 1) * stepX + stepX / 2.0 + offsetX;
			double y = (fromRow - 1) * stepY + stepY / 2.0 + offsetY;

			int dx = (toCol - fromCol) * stepX;
			int dy = (toRow - fromRow) * stepY;

			double length = Math.Sqrt(dx * dx + dy * dy);
			double angle = Math.Atan2(dy, dx) * 180.0 / Math.PI;

			return $"left:{x}px; top:{y}px; width:{length}px; transform:rotate({angle}deg);";
		}
	}
}
