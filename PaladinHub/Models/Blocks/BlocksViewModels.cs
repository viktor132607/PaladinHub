using System;
using System.Collections.Generic;

namespace PaladinHub.Models.Blocks
{
	// ---------- Общи reference типове ----------
	public record ItemRef(int? Id = null, string? Name = null);
	public record SpellRef(int? Id = null, string? Name = null, string? Note = null);

	public record BuildInfo(string Name, bool IsDefault);
	public record TabPane(string Title, string? Html = null); // за Tabs/Switcher, Html ще го рендерим директно

	public record GearRow(string Slot, ItemRef Item, string Source);
	public record ConsumableRow(string Type, ItemRef Best, ItemRef? Alternative = null);

	public record TableColumn(string Key, string Title, string Kind = "text"); // Kind: text|item|spell
	public record TierEntry(string Type, int? Id = null, string? Name = null); // Type: item|spell
	public record SwitcherOption(string Label, string? Html = null);

	// ========== Базов блок (по избор за разширяване) ==========
	public abstract class BlockVM
	{
		/// <summary>Уникално id на секцията (за anchor/навигация).</summary>
		public string SectionId { get; set; } = $"block-{Guid.NewGuid():N}";
		/// <summary>Неформално заглавие (ако блокът има такова).</summary>
		public string? Title { get; set; }
	}

	// ========== Divider / Separator ==========
	public class DividerBlock : BlockVM
	{
		/// <summary>D1..D5 (изображение от ~/images/Separators/).</summary>
		public string Style { get; set; } = "D1";
		public string? CssClass { get; set; }
	}

	// ========== Page Header ==========
	public class PageHeaderBlock : BlockVM
	{
		public string IconUrl { get; set; } = "/images/itemIcons/inv_scroll_03.jpg";
		public bool SmallIcon { get; set; } = false;
	}

	// ========== Heading ==========
	public class HeadingBlock : BlockVM
	{
		/// <summary>h1|h2|h3</summary>
		public string Level { get; set; } = "h2";
		/// <summary>left|center|right</summary>
		public string Align { get; set; } = "left";
		public string? CssClass { get; set; }
		/// <summary>Текстът на заглавието (с поддръжка на минимален HTML).</summary>
		public string Text { get; set; } = "Heading";
	}

	// ========== Tabs ==========
	public class TabsBlock : BlockVM
	{
		public List<TabPane> Tabs { get; set; } = new();
	}

	// ========== Switcher (бутонен) ==========
	public class SwitcherBlock : BlockVM
	{
		public List<SwitcherOption> Options { get; set; } = new();
	}

	// ========== Таблица: Gear ==========
	public class GearTableBlock : BlockVM
	{
		public List<GearRow> Rows { get; set; } = new();
	}

	// ========== Таблица: Consumables ==========
	public class ConsumablesTableBlock : BlockVM
	{
		public List<ConsumableRow> Rows { get; set; } = new();
	}

	// ========== Таблица: Generic ==========
	public class GenericTableBlock : BlockVM
	{
		public List<TableColumn> Columns { get; set; } = new();

		/// <summary>ID на DataPreset, ако таблицата се пълни автоматично.</summary>
		public int? PresetId { get; set; }

		/// <summary>Редове от пресета или ръчно зададени; ключ = колоната (Columns[i].Key).</summary>
		public IReadOnlyList<Dictionary<string, object?>>? Rows { get; set; }
	}

	// ========== Tier List ==========
	public class TierListBlock : BlockVM
	{
		public List<string> Tiers { get; set; } = new() { "S", "A", "B", "C" };
		public Dictionary<string, List<TierEntry>> ItemsByTier { get; set; } = new();
	}

	// ========== Текст с колони ==========
	public class ColumnsTextBlock : BlockVM
	{
		/// <summary>2..4</summary>
		public int Columns { get; set; } = 2;
		/// <summary>Markdown/HTML на колона.</summary>
		public List<string> MarkdownPerColumn { get; set; } = new();
	}

	// ========== Talent Tree ==========
	public class TalentTreeBlock : BlockVM
	{
		/// <summary>"paladin" | "holy" | "holy-herald" | ...</summary>
		public string TreeKey { get; set; } = "paladin";
		/// <summary>Име на билд (по избор) — ако липсва, ще се използва default билд; ако няма билд → всичко сиво.</summary>
		public string? Build { get; set; }
	}

	// ========== Talent Build Menu ==========
	public class TalentBuildMenuBlock : BlockVM
	{
		public string TreeKey { get; set; } = "paladin";
		public List<BuildInfo> Builds { get; set; } = new();
		public string? Selected { get; set; }
	}

	// ========== Item Grid ==========
	public class ItemGridBlock : BlockVM
	{
		public int Columns { get; set; } = 4;
		public List<ItemRef> Items { get; set; } = new();
	}

	// ========== Spell List ==========
	public class SpellListBlock : BlockVM
	{
		public List<SpellRef> Spells { get; set; } = new();
	}

	// ========== Callout ==========
	public class CalloutBlock : BlockVM
	{
		/// <summary>info|tip|warn|success</summary>
		public string Variant { get; set; } = "info";
		/// <summary>Текст (може и с лек HTML/Markdown).</summary>
		public string Text { get; set; } = string.Empty;
	}

	// ========== Rotation Card ==========
	public class RotationCardBlock : BlockVM
	{
		public List<SpellRef> Sequence { get; set; } = new();
	}

	// ========== Section (обвивка) ==========
	public class SectionBlock : BlockVM
	{
		public string? CssClass { get; set; } = "mb-4";
		public string? Html { get; set; } // за вграждане на произволен HTML (по-рядко)
	}
}
