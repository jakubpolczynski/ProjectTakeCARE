using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;

namespace TakeCare.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class VisitController : Controller
	{
		private readonly IVisitService? _visitService;

		public VisitController(IVisitService visitService)
		{
			_visitService = visitService;
		}

		[HttpPost("FindAvailableVisits")]
		public async Task<IActionResult> FindAvailableVisits(FindDateDto findDate)
		{
			var firstName = findDate.FirstName;
			var lastName = findDate.LastName;
			var specialization = findDate.Specialization;
			var date = findDate.Date;

			if (string.IsNullOrWhiteSpace(specialization) || date == default)
			{
				return BadRequest("Invalid input parameters.");
			}

			try
			{
				var availableVisits = await _visitService!.FindAvailableDate(firstName, lastName, specialization, date);
				return Ok(availableVisits);
			}
			catch (ArgumentException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
