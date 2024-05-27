using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Data;
using TakeCare.Application.Models;

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
	}
}
