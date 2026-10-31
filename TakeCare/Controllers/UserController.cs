using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Models;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;
using TakeCare.Security;

namespace TakeCare.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class UserController(TakeCareDbContext database) : ControllerBase
{
    private static bool SafePassword(string value) => Encoding.UTF8.GetByteCount(value) is >= 8 and <= 72;

    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("login")]
    [HttpPost("AddDoctor")]
    public async Task<IActionResult> AddDoctor(DoctorDto doctor)
    {
        if (!SafePassword(doctor.Password)) return BadRequest(new { error = "Invalid password length." });
        if (await database.UserTable!.AnyAsync(u => u.Email == doctor.Email)) return Conflict(new { error = "Account cannot be created." });
        var entity = new Doctor
        {
            FirstName = doctor.FirstName, LastName = doctor.LastName, Specialization = doctor.Specialization,
            Phone = doctor.Phone, Email = doctor.Email,
            User = new User { Email = doctor.Email, Password = BCrypt.Net.BCrypt.HashPassword(doctor.Password, 12), Role = "Doctor" }
        };
        database.DoctorTable!.Add(entity);
        await database.SaveChangesAsync();
        return StatusCode(201, new { id = entity.User.Id });
    }

    [AllowAnonymous]
    [EnableRateLimiting("login")]
    [HttpPost("AddPatient")]
    public async Task<IActionResult> AddPatient(PatientDto patient)
    {
        if (!SafePassword(patient.Password)) return BadRequest(new { error = "Invalid password length." });
        if (await database.UserTable!.AnyAsync(u => u.Email == patient.Email) ||
            await database.PatientTable!.AnyAsync(p => p.Pesel == patient.Pesel))
            return Conflict(new { error = "Account cannot be created." });
        var entity = new Patient
        {
            Pesel = patient.Pesel, FirstName = patient.FirstName, LastName = patient.LastName,
            Email = patient.Email, Phone = patient.Phone,
            Address = new Address { City = patient.City, Street = patient.Street, PostalCode = patient.PostalCode },
            User = new User { Email = patient.Email, Password = BCrypt.Net.BCrypt.HashPassword(patient.Password, 12), Role = "Patient" }
        };
        database.PatientTable!.Add(entity);
        await database.SaveChangesAsync();
        return StatusCode(201, new { id = entity.User.Id });
    }

    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("login")]
    [HttpPost("AddReceptionist")]
    public async Task<IActionResult> AddReceptionist(ReceptionistDto receptionist)
    {
        if (!SafePassword(receptionist.Password)) return BadRequest(new { error = "Invalid password length." });
        if (await database.UserTable!.AnyAsync(u => u.Email == receptionist.Email)) return Conflict(new { error = "Account cannot be created." });
        var entity = new Receptionist
        {
            FirstName = receptionist.FirstName, LastName = receptionist.LastName, Email = receptionist.Email,
            User = new User { Email = receptionist.Email, Password = BCrypt.Net.BCrypt.HashPassword(receptionist.Password, 12), Role = "Receptionist" }
        };
        database.ReceptionistTable!.Add(entity);
        await database.SaveChangesAsync();
        return StatusCode(201, new { id = entity.User.Id });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (id == ResourceAccess.UserId(User)) return BadRequest(new { error = "Self-deletion is disabled." });
        var user = await database.UserTable!.FindAsync(id);
        if (user is null) return NotFound();
        database.UserTable.Remove(user);
        await database.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetUser(int id)
    {
        if (id != ResourceAccess.UserId(User) && !User.IsInRole("Admin")) return Forbid();
        var user = await database.UserTable!.AsNoTracking().Where(u => u.Id == id)
            .Select(u => new { u.Id, u.Email, u.Role }).SingleOrDefaultAsync();
        return user is null ? NotFound() : Ok(user);
    }

    // Only the authenticated user's profile may be updated.
    // Email, role, identifiers and password from the legacy DTO are deliberately not assigned.
    [Authorize(Roles = "Patient")]
    [HttpPost("UpdatePatient")]
    public async Task<IActionResult> UpdatePatient(PatientDto patient)
    {
        var id = ResourceAccess.UserId(User);
        var entity = await database.PatientTable!.Include(p => p.Address).SingleOrDefaultAsync(p => p.UserId == id);
        if (entity?.Address is null) return NotFound();
        entity.FirstName = patient.FirstName; entity.LastName = patient.LastName; entity.Phone = patient.Phone;
        entity.Address.City = patient.City; entity.Address.Street = patient.Street; entity.Address.PostalCode = patient.PostalCode;
        await database.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost("UpdateDoctor")]
    public async Task<IActionResult> UpdateDoctor(DoctorDto doctor)
    {
        var id = ResourceAccess.UserId(User);
        var entity = await database.DoctorTable!.SingleOrDefaultAsync(d => d.UserId == id);
        if (entity is null) return NotFound();
        entity.FirstName = doctor.FirstName; entity.LastName = doctor.LastName; entity.Phone = doctor.Phone;
        entity.Specialization = doctor.Specialization;
        await database.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPost("UpdateReceptionist")]
    public async Task<IActionResult> UpdateReceptionist(ReceptionistDto receptionist)
    {
        var id = ResourceAccess.UserId(User);
        var entity = await database.ReceptionistTable!.SingleOrDefaultAsync(r => r.UserId == id);
        if (entity is null) return NotFound();
        entity.FirstName = receptionist.FirstName; entity.LastName = receptionist.LastName;
        await database.SaveChangesAsync();
        return NoContent();
    }
}
