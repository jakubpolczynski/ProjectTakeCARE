using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Data;
using TakeCare.Application.Models;
using TakeCare.Database.Entity;

namespace TakeCare.Application.Services
{
	public class VisitService : IVisitService
	{
		private readonly TakeCareDbContext _context;

		public VisitService(TakeCareDbContext context)
		{
			_context = context;
		}

		public async Task<AvailableDateDto> FindAvailableDate(string? firstName, string? lastName, string specialization, DateTime date)
		{
			var startHour = 9;
			var endHour = 17;

			var doctorsQuery = _context.DoctorTable!.AsQueryable();

			if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
			{
				doctorsQuery = doctorsQuery.Where(d => d.FirstName == firstName && d.LastName == lastName);
			}

			var doctors = await doctorsQuery.Where(d => d.Specialization == specialization).ToListAsync();

			if (!doctors.Any())
			{
				throw new ArgumentException("No doctors found for the provided criteria.");
			}

			var doctorDtos = new List<AvailableDateDoctorDto>();

			foreach (var doctor in doctors)
			{
				var allSlots = Enumerable.Range(startHour, endHour - startHour)
										  .Select(hour => new DateTime(date.Year, date.Month, date.Day, hour, 0, 0, DateTimeKind.Local))
										  .ToList();

				var appointments = await _context.VisitTable!
					.Where(v => v.DoctorId == doctor.Id && v.Date.Date == date.Date)
					.Select(v => v.Date)
					.ToListAsync();

				var availableSlots = allSlots.Except(appointments).ToList();

				doctorDtos.Add(new AvailableDateDoctorDto
				{
					FirstName = doctor.FirstName,
					LastName = doctor.LastName,
					Specialization = doctor.Specialization,
					Phone = doctor.Phone,
					Email = doctor.Email,
					AvailableSlots = availableSlots
				});
			}

			return new AvailableDateDto
			{
				Doctors = doctorDtos
			};
		}

		public async Task BookVisit(VisitDto visit)
		{
			if (visit == null)
			{
				throw new ArgumentNullException(nameof(visit));
			}

			var doctor = await _context.DoctorTable!
				.Include(d => d.Visits)
				.FirstOrDefaultAsync(d => d.Email == visit.DoctorEmail) ?? throw new ArgumentException("Doctor not found.");

			var slotDate = visit.Slot;
			if (doctor.Visits != null && doctor.Visits.Any(v => v.Date == slotDate))
			{
				throw new InvalidOperationException("Doctor is not available at the requested slot.");
			}

			var patient = await _context.PatientTable!
				.FirstOrDefaultAsync(p => p.Email == visit.PatientEmail);

			if (patient == null)
			{
				throw new ArgumentException("Patient not found.");
			}

			var newVisit = new Visit
			{
				DoctorId = doctor.Id,
				PatientId = patient.Id,
				Date = visit.Slot,
				Description = visit.Description
			};

			_context.VisitTable!.Add(newVisit);
			await _context.SaveChangesAsync();
		}

		public async Task<List<VisitDto>> GetPatientVisits(string patientEmail)
		{
			if (string.IsNullOrWhiteSpace(patientEmail))
			{
				throw new ArgumentException("Patient email is required.");
			}

			var patient = await _context.PatientTable!
				.Include(p => p.Visits!)
				.ThenInclude(v => v.Doctor)
				.FirstOrDefaultAsync(p => p.Email == patientEmail) ?? throw new ArgumentException("Patient not found.");

			var visitDtos = patient.Visits?.Select(v => new VisitDto
			{
				DoctorEmail = v.Doctor!.Email,
				Slot = v.Date,
				PatientEmail = patient.Email,
				Description = v.Description,
				DoctorFirstName = v.Doctor.FirstName,
				DoctorLastName = v.Doctor.LastName,
				DoctorSpecialization = v.Doctor.Specialization
			}).ToList();

			return visitDtos ?? new List<VisitDto>();
		}
		public async Task<List<VisitDto>> GetDoctorVisits(string doctorEmail) {
			if (string.IsNullOrWhiteSpace(doctorEmail))
			{
				throw new ArgumentException("Doctor email is required.");
			}

			var doctor = await _context.DoctorTable!
				.Include(p => p.Visits!)
				.ThenInclude(v => v.Patient)
				.FirstOrDefaultAsync(p => p.Email == doctorEmail) ?? throw new ArgumentException("Doctor not found.");

			var visitDtos = doctor.Visits?.Select(v => new VisitDto
			{
				DoctorEmail = doctor.Email ,
				Slot = v.Date,
				PatientEmail = v.Patient!.Email,
				Description = v.Description,
				DoctorFirstName = doctor.FirstName,
				DoctorLastName = doctor.LastName,
				DoctorSpecialization = doctor.Specialization
			}).ToList();

			return visitDtos ?? new List<VisitDto>();
		}
		public async Task DeleteBookedVisit(VisitDto visit)
		{
			if (visit == null)
			{
				throw new ArgumentNullException(nameof(visit));
			}

			var doctor = await _context.DoctorTable!.FirstOrDefaultAsync(d => d.Email == visit.DoctorEmail) ?? throw new ArgumentException("Doctor not found");

			var patient = await _context.PatientTable!.FirstOrDefaultAsync(p => p.Email == visit.PatientEmail) ?? throw new ArgumentException("Patient not found");

			var bookedVisit = await _context.VisitTable!
				.FirstOrDefaultAsync(
					v => v.DoctorId == doctor!.Id && v.Date == visit.Slot && v.PatientId == patient!.Id
				) ?? throw new ArgumentException("Booked visit not found.");
			
			_context.VisitTable!.Remove(bookedVisit);
			await _context.SaveChangesAsync();
		}

	}
}
