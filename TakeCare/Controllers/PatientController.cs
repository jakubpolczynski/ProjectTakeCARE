using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Security;

namespace TakeCare.Controllers;

[ApiController]
[Authorize(Roles = "Doctor")]
[Route("api/[controller]")]
public sealed class PatientController(IPatientService patients, ResourceAccess access) : ControllerBase
{
    [HttpGet("GetDoctorPatients")]
    public async Task<IActionResult> GetDoctorPatients(string? email)
    {
        var ownEmail = await access.DoctorEmail(User);
        if (ownEmail is null || (email is not null && email != ownEmail)) return Forbid();
        return Ok(await patients.GetPatients(ResourceAccess.UserId(User)));
    }
}
