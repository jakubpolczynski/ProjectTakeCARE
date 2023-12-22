using TakeCare.Application.Interfaces;
using TakeCare.Database.Entity;

namespace TakeCare.Application.Services
{
	public class DoctorService : IDoctorService
	{
		public Task<bool> CheckIfDoctorExistAsync(int doctorId)
		{
			throw new NotImplementedException();
		}

		public Task<string> CreateDoctorAsync(Doctor doctor)
		{
			throw new NotImplementedException();
		}

		public Task<string> DeleteDoctorAsync(int doctorId)
		{
			throw new NotImplementedException();
		}

		public Task<Doctor> ReadDoctorAsync(int doctorId)
		{
			throw new NotImplementedException();
		}

		public Task<string> UpdateDoctorAsync(Doctor doctor)
		{
			throw new NotImplementedException();
		}
	}
}
