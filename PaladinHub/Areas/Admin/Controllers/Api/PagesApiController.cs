using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaladinHub.Services.PageBuilder;

namespace PaladinHub.Areas.Admin.Controllers.Api
{
	[Area("Admin")]
	[ApiController]
	[Route("Admin/api/pages")]
	[Authorize(Roles = "Admin")]
	public sealed class PagesApiController : ControllerBase
	{
		private readonly PaladinHub.Services.PageBuilder.IPageService _pages;

		public PagesApiController(PaladinHub.Services.PageBuilder.IPageService pages) => _pages = pages;

		public sealed record PutLayoutRequest(string JsonLayout, string RowVersionBase64);

		[HttpPut("{id:int}/layout")]
		public async Task<IActionResult> PutLayout(int id, [FromBody] PutLayoutRequest req)
		{
			if (id <= 0) return BadRequest(new { message = "Invalid id." });
			if (req is null || string.IsNullOrWhiteSpace(req.JsonLayout))
				return BadRequest(new { message = "JsonLayout is required." });

			byte[] rowVersion;
			try { rowVersion = Convert.FromBase64String(req.RowVersionBase64 ?? ""); }
			catch { return BadRequest(new { message = "RowVersionBase64 is invalid." }); }

			try
			{
				var (ok, newRv) = await _pages.UpdateLayoutSafeAsync(id, req.JsonLayout, rowVersion, User?.Identity?.Name ?? "admin");
				if (!ok && newRv is null)
				{
					return Conflict(new { message = "The page was modified by someone else. Refresh and try again." });
				}

				return Ok(new { rowVersionBase64 = Convert.ToBase64String(newRv!) });
			}
			catch (JsonLayoutValidationException ex)
			{
				return BadRequest(new { message = "Layout validation failed.", errors = ex.Errors });
			}
			catch (JsonException ex)
			{
				return BadRequest(new { message = "Invalid JSON.", error = ex.Message });
			}
		}

		[HttpGet("{id:int}/head")]
		public async Task<IActionResult> GetHead(int id)
		{
			var page = await _pages.GetByIdAsync(id);
			if (page is null) return NotFound();
			return Ok(new { id = page.Id, rowVersionBase64 = Convert.ToBase64String(page.RowVersion), updatedAt = page.UpdatedAt });
		}
	}
}
