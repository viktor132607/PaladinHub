using Microsoft.AspNetCore.Mvc;
using PaladinHub.Models.Talents;
using PaladinHub.Services.TalentTrees;

namespace PaladinHub.Controllers.Api
{
	[ApiController]
	[Route("api/talents")]
	public class TalentsApiController : ControllerBase
	{
		private readonly ITalentTreeService _trees;
		public TalentsApiController(ITalentTreeService trees) => _trees = trees;

		[HttpPost("{key}")]
		public async Task<IActionResult> Save(string key, [FromBody] SaveTreeRequest req)
		{
			if (req == null || key != req.Key) return BadRequest();

			await _trees.SaveActiveStatesAsync(req.Key, req.Nodes);

			return NoContent();
		}
	}
}
