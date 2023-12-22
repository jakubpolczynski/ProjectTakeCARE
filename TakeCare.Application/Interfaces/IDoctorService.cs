using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
	public interface IDoctorService
	{
		Task<string> CreateDoctorAsync(Doctor doctor);

		Task<Doctor> ReadDoctorAsync(int doctorId);

		Task<string> UpdateDoctorAsync(Doctor doctor);

		Task<string> DeleteDoctorAsync(int doctorId);

		Task<bool> CheckIfDoctorExistAsync(int doctorId);
	}
}
