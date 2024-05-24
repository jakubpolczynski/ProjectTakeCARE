using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Data;

namespace TakeCare.Application.Services
{
	public class VisitService : IVisitService
	{
		private readonly TakeCareDbContext _context;

		public VisitService(TakeCareDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<DateTime>> FindAvailableDate(string firstName, string lastName, DateTime date)
		{
			var startHour = 9;
			var endHour = 17;

			var doctor = await _context.DoctorTable!
				.FirstOrDefaultAsync(d => d.FirstName == firstName && d.LastName == lastName);

			if (doctor == null)
			{
				throw new ArgumentException("Doctor not found.");
			}

			var doctorId = doctor.Id;

			var allSlots = Enumerable.Range(startHour, endHour - startHour)
									  .Select(hour => new DateTime(date.Year, date.Month, date.Day, hour, 0, 0, DateTimeKind.Local))
									  .ToList();

			var appointments = await _context.VisitTable!
				.Where(v => v.DoctorId == doctorId && v.Date.Date == date.Date)
				.Select(v => v.Date)
				.ToListAsync();

			var availableSlots = allSlots.Except(appointments).ToList();

			return availableSlots;
		}
	}

	public class Appointment
	{
		public string? DoctorFirstName { get; set; }
		public string? DoctorLastName { get; set; }
		public DateTime Date { get; set; }
	}
}
