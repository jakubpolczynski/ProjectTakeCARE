using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Models;

namespace TakeCare.Controllers
{
	public class VisitController : Controller
	{
		private readonly IVisitService? _visitService;

		public VisitController(IVisitService visitService)
		{
			_visitService = visitService;
		}

		[HttpGet("FindAvilableVisits")]
		public async Task<IActionResult> FindAvilableVisits(FindDateDto findDate)
		{
			var firstName = findDate.FirstName;
			var lastName = findDate.LastName;
			var date = findDate.Date;

			if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || date == default)
			{
				return BadRequest("Invalid input parameters.");
			}

			try
			{
				var availableVisits = await _visitService!.FindAvailableDate(firstName, lastName, date);
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
