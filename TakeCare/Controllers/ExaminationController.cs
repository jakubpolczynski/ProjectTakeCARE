using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;
using TakeCare.Application.Services;
using TakeCare.Database.Data;
using TakeCare.Security;

namespace TakeCare.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class ExaminationController(IExaminationService examinations, IEmailService email,
    ResourceAccess access, TakeCareDbContext database, IConfiguration configuration,
    ILogger<ExaminationController> logger) : ControllerBase
{
    [HttpGet("GetDoctorExaminations")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorExaminations(string? doctorEmail)
    {
        var ownEmail = await access.DoctorEmail(User);
        if (ownEmail is null || (doctorEmail is not null && doctorEmail != ownEmail)) return Forbid();
        return Ok(await examinations.GetDoctorExaminations(ResourceAccess.UserId(User)));
    }

    [HttpGet("{id:int}/pdf")]
    [Authorize(Roles = "Doctor,Patient")]
    public async Task<IActionResult> GetPdf(int id)
    {
        var userId = ResourceAccess.UserId(User);
        var doctor = User.IsInRole("Doctor");
        var patient = User.IsInRole("Patient");
        var content = await database.ExaminationTable!.AsNoTracking()
            .Where(e => e.Id == id && ((doctor && e.Doctor!.UserId == userId) ||
                                       (patient && e.Patient!.UserId == userId)))
            .Select(e => e.PdfContent).SingleOrDefaultAsync();
        if (content is null) return NotFound();
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return File(content, "application/pdf", "examination.pdf");
    }

    [HttpGet("GetPatientExaminations")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientExaminations(string? patientEmail)
    {
        var ownEmail = await access.PatientEmail(User);
        if (ownEmail is null || (patientEmail is not null && patientEmail != ownEmail)) return Forbid();
        return Ok(await examinations.GetPatientExaminations(ResourceAccess.UserId(User)));
    }

    [HttpPost("AddExamination")]
    [Authorize(Roles = "Doctor")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 6 * 1024 * 1024)]
    public async Task<IActionResult> AddExamination([FromForm] ExaminationDto dto, IFormFile? file)
    {
        var userId = ResourceAccess.UserId(User);
        var visit = await database.VisitTable!.AsNoTracking().Include(v => v.Doctor).Include(v => v.Patient)
            .SingleOrDefaultAsync(v => v.Id == dto.VisitId && v.Doctor!.UserId == userId && !v.IsVisitExecuted);
        if (visit?.Doctor is null || visit.Patient is null) return NotFound();
        if (dto.DoctorEmail != visit.Doctor.Email || dto.PatientEmail != visit.Patient.Email) return Forbid();
        // Arbitrary client URLs/paths must not become trusted medical attachments.
        if (dto.Images is { Count: > 0 }) return BadRequest(new { error = "External attachment paths are disabled." });
        if (file is null || file.Length is <= 0 or > 5 * 1024 * 1024 ||
            !string.Equals(file.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = "A PDF of at most 5 MiB is required." });
        byte[] pdfContent;
        using (var stream = file.OpenReadStream())
        using (var output = new MemoryStream())
        {
            var buffer = new byte[81920];
            int read;
            while ((read = await stream.ReadAsync(buffer, HttpContext.RequestAborted)) > 0)
            {
                if (output.Length + read > 5 * 1024 * 1024)
                    return BadRequest(new { error = "PDF exceeds 5 MiB." });
                await output.WriteAsync(buffer.AsMemory(0, read), HttpContext.RequestAborted);
            }
            pdfContent = output.ToArray();
            if (pdfContent.Length < 5 || !pdfContent.AsSpan(0, 5).SequenceEqual("%PDF-"u8))
                return BadRequest(new { error = "Invalid PDF signature." });
        }
        dto.DoctorEmail = visit.Doctor.Email;
        dto.PatientEmail = visit.Patient.Email;
        dto.Date = DateTime.UtcNow;
        int examinationId;
        try { examinationId = await examinations.AddExamination(dto, userId, pdfContent); }
        catch (VisitUnavailableException) { return Conflict(new { error = "Visit is no longer available." }); }
        var delivery = "disabled";
        if (configuration.GetValue<bool>("Email:Enabled"))
        {
            try { await email.SendEmail(dto, file); delivery = "submitted"; }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
            {
                logger.LogWarning("Email submission failed. TraceId={TraceId}", HttpContext.TraceIdentifier);
                delivery = "failed";
            }
        }
        return StatusCode(201, new { id = examinationId, message = "Examination recorded.", emailDelivery = delivery });
    }
}
