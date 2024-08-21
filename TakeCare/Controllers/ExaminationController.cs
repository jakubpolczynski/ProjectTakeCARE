using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;

namespace TakeCare.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExaminationController : Controller
	{
		private readonly IExaminationService? _examinationService;
		private readonly IEmailService? _emailService;
		
		public ExaminationController(IExaminationService examinationService, IEmailService emailService)
		{
			_examinationService = examinationService;
			_emailService = emailService;
		}

		[HttpGet("GetDoctorExaminations")]
		public async Task<ExaminationDto[]> GetDoctorExaminations(string doctorEmail)
		{
			return await _examinationService!.GetDoctorExaminations(doctorEmail);
		}

		[HttpGet("GetPatientExaminations")]
		public async Task<ExaminationDto[]> GetPatientExaminations(string patientEmail)
		{
			return await _examinationService!.GetPatientExaminations(patientEmail);
		}

		[HttpPost("AddExamination")]
		public async Task<IActionResult> AddExamination([FromForm] ExaminationDto examinationDto, IFormFile file)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest("ModelState is not valid");
			}

			if (file.Length == 0)
			{
				return BadRequest(new { error = "File is not valid or missing" });
			}

			await _emailService!.SendEmail(examinationDto, file);
			await _examinationService!.AddExamination(examinationDto);
			return Created("Success", examinationDto);
		}
	}
}
