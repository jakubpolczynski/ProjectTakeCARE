using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
	public interface IPatientService
	{
		Task<string> CreatePatientAsync(Patient patient);

		Task<Patient> ReadPatientAsync(int patientId);

		Task<string> UpdatePatientAsync(Patient patient);

		Task<string> DeletePatientAsync(int patientId);

		Task<bool> CheckIfPatientExistAsync(int patientId);
	}
}
