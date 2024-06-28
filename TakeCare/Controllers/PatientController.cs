using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;
using TakeCare.Application.Services;

namespace TakeCare.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PatientController : Controller
	{
		private readonly IPatientService? _patientService;

		public PatientController(IPatientService patientService)
		{
			_patientService = patientService;
		}

		[HttpGet("GetDoctorPatients")]
		public async Task<IActionResult> GetDoctorPatients([FromQuery] string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return BadRequest("Email is required.");
			}

			try
			{
				var patients = await _patientService!.GetPatients(email);
				if (patients == null || patients.Length == 0)
				{
					return NotFound("No patients found for the specified doctor.");
				}

				return Ok(patients);
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
