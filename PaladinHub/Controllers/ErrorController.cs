using Microsoft.AspNetCore.Mvc;

[Route("error")]
public class ErrorController : Controller
{
	[HttpGet("404")]
	public IActionResult NotFound404()
	{
		Response.StatusCode = 404;
		return View("404");
	}

	[HttpGet("500")]
	public IActionResult InternalServerError()
	{
		Response.StatusCode = 500;
		return View("500");
	}
}
