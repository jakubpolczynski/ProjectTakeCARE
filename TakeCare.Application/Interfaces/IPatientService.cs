namespace TakeCare.Application.Interfaces
{
	public interface IPatientService
	{
		Task<bool> CheckIfPatientExistsAsync(string email);
	}
}