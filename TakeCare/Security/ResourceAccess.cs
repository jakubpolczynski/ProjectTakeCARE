using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TakeCare.Database.Data;

namespace TakeCare.Security;

public sealed class ResourceAccess(TakeCareDbContext database)
{
    public static int UserId(ClaimsPrincipal user) => int.TryParse(user.FindFirstValue("sub"), out var id) ? id : -1;

    public Task<string?> PatientEmail(ClaimsPrincipal user) => database.PatientTable!.AsNoTracking()
        .Where(p => p.UserId == UserId(user)).Select(p => (string?)p.Email).SingleOrDefaultAsync();

    public Task<string?> DoctorEmail(ClaimsPrincipal user) => database.DoctorTable!.AsNoTracking()
        .Where(d => d.UserId == UserId(user)).Select(d => (string?)d.Email).SingleOrDefaultAsync();

    public Task<bool> CanReadVisit(ClaimsPrincipal user, int id) => database.VisitTable!.AsNoTracking()
        .AnyAsync(v => v.Id == id &&
            ((user.IsInRole("Patient") && v.Patient!.UserId == UserId(user)) ||
             (user.IsInRole("Doctor") && v.Doctor!.UserId == UserId(user))));

    public Task<bool> CanCancelVisit(ClaimsPrincipal user, string doctor, string patient, DateTime slot) =>
        database.VisitTable!.AsNoTracking().AnyAsync(v => v.Doctor!.Email == doctor &&
            v.Patient!.Email == patient && v.Date == slot && !v.IsVisitExecuted &&
            ((user.IsInRole("Patient") && v.Patient.UserId == UserId(user)) ||
             (user.IsInRole("Doctor") && v.Doctor.UserId == UserId(user)) || user.IsInRole("Receptionist")));
}
