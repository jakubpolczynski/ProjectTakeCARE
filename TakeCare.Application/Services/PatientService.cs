using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;


namespace TakeCare.Application.Services
{
	public class PatientService : IPatientService
	{
		private readonly TakeCareDbContext _context;
		private readonly DbSet<Patient> _dbSetPatient;
		private readonly DbSet<Visit> _dbSetVisit;
		private readonly DbSet<Doctor> _dbSetDoctor;

		public PatientService(TakeCareDbContext context)
		{
			_context = context;
			_dbSetPatient = _context.Set<Patient>();
			_dbSetVisit = _context.Set<Visit>();
			_dbSetDoctor = _context.Set<Doctor>();
		}

		public async Task<bool> CheckIfPatientExistsAsync(string pesel)
		{
			Patient patientExists;
			if (pesel != null)
			{
				patientExists = await _dbSetPatient.FirstOrDefaultAsync(u => u.Pesel == pesel);
			}
			else
			{
				throw new ArgumentException("Invalid email provided");
			}

			return patientExists != null;
		}

		public async Task<PatientDto[]> GetPatients(string email)
		{
			if (email == null)
			{
				throw new ArgumentException("Invalid doctor specified.");
			}

			var doctor = await _dbSetDoctor.FirstOrDefaultAsync(d => d.Email == email);
			if (doctor == null)
			{
				throw new InvalidOperationException("Doctor not found.");
			}

			var patients = await _dbSetVisit
				.Where(v => v.DoctorId == doctor.Id)
				.Select(v => v.Patient)
				.Distinct()
				.Select(p => new PatientDto
				{
					Role = "Patient",
					FirstName = p.FirstName,
					LastName = p.LastName,
					Phone = p.Phone,
				})
				.ToArrayAsync();

			return patients;
		}
	}
}
