using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace PaladinHub.Infrastructure
{
	/// <summary>
	/// Позволява Razor да търси изгледи в /Views/{section}/{view}.cshtml,
	/// където {section} идва от RouteData.Values["section"].
	/// </summary>
	public sealed class SectionViewLocationExpander : IViewLocationExpander
	{
		private const string SectionKey = "section";

		// Тук записваме стойността на секцията в context.Values, за да важи и за кеша.
		public void PopulateValues(ViewLocationExpanderContext context)
		{
			var hasSection = context.ActionContext.RouteData.Values.TryGetValue(SectionKey, out var value);
			context.Values[SectionKey] = hasSection ? value?.ToString() ?? string.Empty : string.Empty;
		}

		// Добавяме нови пътища за търсене на изгледи, зависещи от секцията.
		public IEnumerable<string> ExpandViewLocations(
			ViewLocationExpanderContext context,
			IEnumerable<string> viewLocations)
		{
			context.Values.TryGetValue(SectionKey, out var section);

			if (!string.IsNullOrEmpty(section))
			{
				// {0}=име на изглед, {1}=контролер, {2}=area
				yield return $"/Views/{section}/{{0}}.cshtml";
				// ако имаш Areas и искаш същото поведение и там:
				yield return $"/Areas/{{2}}/Views/{section}/{{0}}.cshtml";
			}

			// запази стандартните локации след нашите
			foreach (var loc in viewLocations)
				yield return loc;
		}
	}
}
