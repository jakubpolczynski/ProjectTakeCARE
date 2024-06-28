using TakeCare.Application.Models;

namespace TakeCare.Application.Interfaces
{
	public interface IPatientService
	{
		Task<bool> CheckIfPatientExistsAsync(string email);
		Task<PatientDto[]> GetPatients(string email);
	}
}