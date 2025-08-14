using Ganss.Xss;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace PaladinHub.Infrastructure
{
	public static class SafeHtmlExtensions
	{
		public static IHtmlContent SafeHtml(this IHtmlHelper html, string? htmlString)
		{
			var sanitizer = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IHtmlSanitizer>();
			var safe = sanitizer.Sanitize(htmlString ?? string.Empty);
			return new HtmlString(safe);
		}
	}
}
