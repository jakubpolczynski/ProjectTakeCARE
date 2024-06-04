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

		[HttpPost("BookVisit")]
		public async Task<IActionResult> BookVisit(VisitDto visitDto)
		{
			if (visitDto == null)
			{
				return BadRequest("Visit data is required.");
			}

			if (string.IsNullOrWhiteSpace(visitDto.DoctorEmail) ||
				string.IsNullOrWhiteSpace(visitDto.PatientEmail) ||
				visitDto.Slot == default)
			{
				return BadRequest("Invalid input parameters.");
			}

			try
			{
				await _visitService!.BookVisit(visitDto);
				return Ok("Visit booked successfully.");
			}
			catch (ArgumentException ex)
			{
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("GetPatientVisits")]
		public async Task<IActionResult> GetPatientVisits(string patientEmail)
		{
			if (string.IsNullOrWhiteSpace(patientEmail))
			{
				return BadRequest("Patient email is required.");
			}

			try
			{
				var patientVisits = await _visitService!.GetPatientVisits(patientEmail);
				return Ok(patientVisits);
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

		[HttpGet("GetDoctorVisits")]
		public async Task<IActionResult> GetDoctorVisits(string doctorEmail)
		{
			if (string.IsNullOrWhiteSpace(doctorEmail))
			{
				return BadRequest("Doctor email is required.");
			}

			try
			{
				var doctorVisits = await _visitService!.GetDoctorVisits(doctorEmail);
				return Ok(doctorVisits);
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

		[HttpPost("DeleteBookedVisit")]
		public async Task<IActionResult> DeleteBookedVisit(VisitDto visit)
		{
			if(visit == null)
			{
				return BadRequest("Visit data is required.");
			}

			try
			{
				await _visitService!.DeleteBookedVisit(visit);
				return Ok("Visit deleted successfully.");
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
