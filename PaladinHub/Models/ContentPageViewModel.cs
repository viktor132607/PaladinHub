using System.Collections.Generic;
using PaladinHub.Data.Entities; // ако е нужно, смени на PaladinHub.Data.Models

namespace PaladinHub.Models.PageBuilder
{
	/// <summary>
	/// ViewModel за публичния рендер на динамичните страници от Page Builder-а.
	/// (Извън Areas/Admin — тук няма нищо админско.)
	/// </summary>
	public class ContentPageViewModel
	{
		// Основни метаданни
		public string Section { get; set; } = ""; // "Holy", "Protection", "Retribution"
		public string Title { get; set; } = "";
		public string Slug { get; set; } = "";

		// Идентификатори (Id за вътрешно, Uid ако ти трябва стабилен публичен ключ)
		public int Id { get; set; }
		public System.Guid Uid { get; set; }

		/// <summary>
		/// JSON от билдъра. Очаква се масив от блокове: "[]".
		/// </summary>
		public string JsonLayout { get; set; } = "[]";

		/// <summary>
		/// Данни за @Item("…") / @Spell("…") в блоковете.
		/// Пълнят се от твоя глобален филтър/рендерер.
		/// </summary>
		public List<Item> Items { get; set; } = new();
		public List<Spell> Spells { get; set; } = new();

		// ---- добавени за съвместимост с контролера/рендера ----

		/// <summary>Публикувана ли е страницата.</summary>
		public bool IsPublished { get; set; } = true;

		/// <summary>Последна промяна (UTC, по желание показвай локално).</summary>
		public System.DateTime UpdatedAt { get; set; }

		/// <summary>Кой е правил промяната (ако го поддържаш).</summary>
		public string? UpdatedBy { get; set; }

		/// <summary>
		/// RowVersion като Base64 (за optimistic concurrency в Edit/Api).
		/// Не е задължително за публичния рендер, но е удобно да присъства.
		/// </summary>
		public string? RowVersionBase64 { get; set; }
	}
}
