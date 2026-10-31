using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;
using TakeCare.Security;

namespace TakeCare.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class VisitController(IVisitService visits, ResourceAccess access) : ControllerBase
{
    [HttpPost("FindAvailableVisits")]
    [Authorize(Roles = "Patient,Doctor,Receptionist")]
    public async Task<IActionResult> FindAvailableVisits(FindDateDto findDate)
    {
        if (string.IsNullOrWhiteSpace(findDate.Specialization) || findDate.Date == default) return BadRequest();
        return Ok(await visits.FindAvailableDate(findDate.FirstName, findDate.LastName, findDate.Specialization, findDate.Date));
    }

    [HttpPost("BookVisit")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> BookVisit(VisitDto visitDto)
    {
        var email = await access.PatientEmail(User);
        if (email is null || visitDto.PatientEmail != email || visitDto.IsVisitExecuted) return Forbid();
        visitDto.PatientEmail = email;
        visitDto.IsVisitExecuted = false;
        await visits.BookVisit(visitDto, ResourceAccess.UserId(User));
        return Ok(new { message = "Visit booked." });
    }

    [HttpPost("BookVisitAsReceptionist")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> BookVisitAsReceptionist(VisitDto visitDto)
    {
        if (visitDto.IsVisitExecuted) return BadRequest();
        visitDto.IsVisitExecuted = false;
        await visits.BookVisitAsReceptionist(visitDto);
        return Ok(new { message = "Visit booked." });
    }

    [HttpGet("GetPatientVisits")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientVisits(string? patientEmail)
    {
        var email = await access.PatientEmail(User);
        if (email is null || (patientEmail is not null && patientEmail != email)) return Forbid();
        return Ok(await visits.GetPatientVisits(ResourceAccess.UserId(User)));
    }

    [HttpGet("GetDoctorVisits")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorVisits(string? doctorEmail)
    {
        var email = await access.DoctorEmail(User);
        if (email is null || (doctorEmail is not null && doctorEmail != email)) return Forbid();
        return Ok(await visits.GetDoctorVisits(ResourceAccess.UserId(User)));
    }

    [HttpPost("DeleteBookedVisit")]
    [Authorize(Roles = "Patient,Doctor,Receptionist")]
    public async Task<IActionResult> DeleteBookedVisit(VisitDto visit)
    {
        if (!await visits.DeleteBookedVisit(visit, ResourceAccess.UserId(User), User.FindFirst("role")?.Value ?? ""))
            return NotFound();
        return Ok(new { message = "Visit cancelled." });
    }

    [HttpGet("GetVisit")]
    [Authorize(Roles = "Patient,Doctor")]
    public async Task<IActionResult> GetVisit(int id)
    {
        if (!await access.CanReadVisit(User, id)) return NotFound();
        return Ok(await visits.GetVisit(id));
    }

    [HttpGet("GetPatientExecutedVisits")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientExecutedVisits(string? patientEmail)
    {
        var email = await access.PatientEmail(User);
        if (email is null || (patientEmail is not null && patientEmail != email)) return Forbid();
        return Ok(await visits.GetPatientExecutedVisits(ResourceAccess.UserId(User)));
    }

    [HttpGet("GetDoctorExecutedVisits")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorExecutedVisits(string? doctorEmail)
    {
        var email = await access.DoctorEmail(User);
        if (email is null || (doctorEmail is not null && doctorEmail != email)) return Forbid();
        return Ok(await visits.GetDoctorExecutedVisits(ResourceAccess.UserId(User)));
    }
}
