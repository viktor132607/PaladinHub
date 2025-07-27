using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using PaladinHub.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaladinHub.Views.Shared
{
	public abstract class SpellsAndItemsView : RazorPage<PaladinHub.Models.CombinedViewModel>
	{
		protected Dictionary<string, Spell> SpellsByName =>
			Model.Spells?
				.Where(s => !string.IsNullOrWhiteSpace(s.Name))
				.GroupBy(s => s.Name!.Trim(), StringComparer.OrdinalIgnoreCase)
				.Select(g => g.First())
				.ToDictionary(s => s.Name!, s => s, StringComparer.OrdinalIgnoreCase)
			?? new Dictionary<string, Spell>();

		protected Dictionary<string, Item> ItemsByName =>
			Model.Items?
				.Where(i => !string.IsNullOrWhiteSpace(i.Name))
				.GroupBy(i => i.Name!.Trim(), StringComparer.OrdinalIgnoreCase)
				.Select(g => g.First())
				.ToDictionary(i => i.Name!, i => i, StringComparer.OrdinalIgnoreCase)
			?? new Dictionary<string, Item>();

		protected IHtmlContent Spell(string spellName, int size = 20)
		{
			if (!SpellsByName.ContainsKey(spellName))
				return HtmlString.Empty;

			var spell = SpellsByName[spellName];
			var qualityClass = spell.Quality?.ToLower() ?? "common";
			var url = string.IsNullOrWhiteSpace(spell.Url) ? "#" : spell.Url;

			var html = $@"
            <a href='{url}' target='_blank' class='item-link item {qualityClass}' style='display: inline-flex; align-items: center; gap: 6px; text-decoration: none;'>
                <img src='/images/SpellIcons/{spell.Icon}' alt='{spell.Name}' title='{spell.Name}' width='{size}' height='{size}' style='vertical-align: middle;' />
                <span>{spell.Name}</span>
            </a>";

			return new HtmlString(html);
		}

		protected IHtmlContent Item(string itemName, int size = 20)
		{
			if (!ItemsByName.ContainsKey(itemName))
				return HtmlString.Empty;

			var item = ItemsByName[itemName];
			var qualityClass = item.Quality?.ToLower() ?? "common";
			var url = string.IsNullOrWhiteSpace(item.Url) ? "#" : item.Url;

			var secondIconHtml = string.IsNullOrWhiteSpace(item.SecondIcon)
				? ""
				: $"<img src='/images/ItemIcons/{item.SecondIcon}' title='{item.Name}' width='{size}' height='{size}' style='vertical-align: middle;' />";

			var html = $@"
		<a href='{url}' target='_blank' class='item-link item {qualityClass}' 
		   style='display: inline-flex; align-items: center; gap: 6px; text-decoration: none;'>
			<img src='/images/ItemIcons/{item.Icon}' alt='{item.Name}' width='{size}' height='{size}' style='vertical-align: middle;' />
			<span>{item.Name}</span>
			{secondIconHtml}
		</a>";

			return new HtmlString(html);
		}


		protected IHtmlContent SpellNode(string spellName, int col, int row, string shape = "circle")
		{
			if (!SpellsByName.ContainsKey(spellName))
				return HtmlString.Empty;

			var spell = SpellsByName[spellName];
			var shapeClass = $"node-{shape.ToLower()}";

			var html = $@"
			<div class='node {shapeClass}' style='grid-column: {col}; grid-row: {row};'>
				<a href='{spell.Url}' target='_blank'>
					<img src='/images/SpellIcons/{spell.Icon}' alt='{spell.Name}' />
				</a>
			</div>";

			return new HtmlString(html);
		}

		protected IHtmlContent Line(int fromCol, int fromRow, int toCol, int toRow, string fromShape = "circle", string toShape = "circle")
		{
			double offsetX = GetOffset(fromShape).x;
			double offsetY = GetOffset(fromShape).y;

			var style = LineStyle(fromCol, fromRow, toCol, toRow, offsetX, offsetY);
			var html = $"<div class='line' style='{style}'></div>";
			return new HtmlString(html);
		}

		protected string LineStyle(int fromCol, int fromRow, int toCol, int toRow, double offsetX = 7.5, double offsetY = 7.5)
		{
			int cellW = 50;
			int cellH = 60;
			int gap = 20;

			int stepX = cellW + gap;
			int stepY = cellH + gap;

			double x = (fromCol - 1) * stepX + stepX / 2 + offsetX;
			double y = (fromRow - 1) * stepY + stepY / 2 + offsetY;

			int dx = (toCol - fromCol) * stepX;
			int dy = (toRow - fromRow) * stepY;

			double length = Math.Sqrt(dx * dx + dy * dy);
			double angle = Math.Atan2(dy, dx) * (180.0 / Math.PI);

			return $"top: {y}px; left: {x}px; width: {length}px; height: 4px; transform: rotate({angle}deg); transform-origin: top left;";
		}

		private (double x, double y) GetOffset(string shape)
		{
			switch (shape.ToLower())
			{
				case "hexagon":
				case "square":
					return (-7.5, -7.5);
				default:
					return (-7.5, -7.5);
			}
		}
	}
}
