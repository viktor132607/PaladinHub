using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using PaladinHub.Data.Entities;

namespace PaladinHub.Views.Shared
{
	public abstract class SpellsAndItemsView : RazorPage<PaladinHub.Models.CombinedViewModel>
	{
		protected IReadOnlyDictionary<string, Spell> SpellsByName =>
			(Model?.Spells ?? Enumerable.Empty<Spell>())
				.Where(s => !string.IsNullOrWhiteSpace(s.Name))
				.GroupBy(s => s.Name!.Trim(), StringComparer.OrdinalIgnoreCase)
				.Select(g => g.First())
				.ToDictionary(s => s.Name!.Trim(), StringComparer.OrdinalIgnoreCase);

		protected IReadOnlyDictionary<string, PaladinHub.Data.Entities.Item> ItemsByName =>
			(Model?.Items ?? Enumerable.Empty<PaladinHub.Data.Entities.Item>())
				.Where(i => !string.IsNullOrWhiteSpace(i.Name))
				.GroupBy(i => i.Name!.Trim(), StringComparer.OrdinalIgnoreCase)
				.Select(g => g.First())
				.ToDictionary(i => i.Name!.Trim(), StringComparer.OrdinalIgnoreCase);

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
			if (!SpellsByName.TryGetValue(spellName, out var spell))
				return HtmlString.Empty;

			var url = string.IsNullOrWhiteSpace(spell.Url) ? "#" : spell.Url!;
			var icon = ResolveIconPath(spell.Icon, "SpellIcons");
			var qualityClass = (spell.Quality ?? "spell").ToLowerInvariant();

			var html = $@"
<span class='spell-ref'>
  <a href='{url}' target='_blank' class='spell-link {qualityClass}' style='display:inline-flex;align-items:center;gap:6px;text-decoration:none;'>
    <img src='{icon}' alt='{spell.Name}' width='{size}' height='{size}' style='vertical-align:middle;' />
    <span>{spell.Name}</span>
  </a>
</span>";
			return new HtmlString(html);
		}

		protected IHtmlContent ItemLink(string itemName, int size = 20)
		{
			if (!ItemsByName.TryGetValue(itemName, out var item))
				return HtmlString.Empty;

			var url = string.IsNullOrWhiteSpace(item.Url) ? "#" : item.Url!;
			var icon1 = ResolveIconPath(item.Icon, "ItemIcons");
			var icon2 = ResolveIconPath(item.SecondIcon, "ItemIcons", "");
			var qualityClass = (item.Quality ?? "common").ToLowerInvariant();

			var secondIcon = string.IsNullOrWhiteSpace(item.SecondIcon)
				? ""
				: $"<img src='{icon2}' title='{item.SecondIcon}' width='{size}' height='{size}' style='vertical-align:middle;' />";

			var html = $@"
			<span class='item-ref'>
			  <a href='{url}' target='_blank' class='item-link item {qualityClass}' style='display:inline-flex;align-items:center;gap:6px;text-decoration:none;'>
			    <img src='{icon1}' alt='{item.Name}' width='{size}' height='{size}' style='vertical-align:middle;' />
			    <span>{item.Name}</span>
			    {secondIcon}
			  </a>
			</span>";
			return new HtmlString(html);
		}

		protected IHtmlContent Item(string itemName, int size = 20) => ItemLink(itemName, size);
		protected IHtmlContent Spell(string spellName, int size = 20) => SpellLink(spellName, size);
		protected IHtmlContent item(string itemName, int size = 20) => ItemLink(itemName, size);
		protected IHtmlContent spell(string spellName, int size = 20) => SpellLink(spellName, size);

		// ===== Нод от ViewModel (динамични дървета) =====
		protected IHtmlContent SpellNode(PaladinHub.Models.Talents.TalentNodeViewModel n)
		{
			var name = n.SpellName ?? string.Empty;
			Spell? spell = null;
			if (!string.IsNullOrWhiteSpace(name))
			{
				if (!SpellsByName.TryGetValue(name, out spell))
					return HtmlString.Empty;
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
			if (!SpellsByName.TryGetValue(spellName, out var spell))
				return HtmlString.Empty;

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

		// ===== Линии – НУЛЕВ офсет (x/y) =====
		protected IHtmlContent Line(int fromCol, int fromRow, int toCol, int toRow, double offsetX = -8, double offsetY = -8)
		{
			var style = LineStyle(fromCol, fromRow, toCol, toRow, offsetX, offsetY);
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

		// ===== Старият overload: по „форми“ – също без офсет =====
		protected IHtmlContent Line(int fromCol, int fromRow, int toCol, int toRow, string fromShape, string toShape)
		{
			var (ox, oy) = GetOffset(fromShape);
			return Line(fromCol, fromRow, toCol, toRow, ox, oy);
		}

		private static (double x, double y) GetOffset(string? shape)
		{
			// Нулев офсет
			return (0, 0);
		}
	}
}
