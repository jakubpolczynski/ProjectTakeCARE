using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;


namespace TakeCare.Application.Services
{
	public class PatientService : IPatientService
	{
		private readonly TakeCareDbContext _context;
		private readonly DbSet<Patient> _dbSet;

		public PatientService(TakeCareDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<Patient>();
		}

		public async Task<bool> CheckIfPatientExistsAsync(string pesel)
		{
			Patient patientExists;
			if (pesel != null)
			{
				patientExists = await _dbSet.FirstOrDefaultAsync(u => u.Pesel == pesel);
			}
			else
			{
				throw new ArgumentException("Invalid email provided");
			}

			return patientExists != null;
		}
	}
}
