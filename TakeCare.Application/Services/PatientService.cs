using TakeCare.Application.Interfaces;
using TakeCare.Database.Entity;

namespace TakeCare.Application.Services
{
	internal class PatientService : IPatientService
	{
		public Task<bool> CheckIfPatientExistAsync(int patientId)
		{
			throw new NotImplementedException();
		}

		public Task<string> CreatePatientAsync(Patient patient)
		{
			throw new NotImplementedException();
		}

		public Task<string> DeletePatientAsync(int patientId)
		{
			throw new NotImplementedException();
		}

		public Task<Patient> ReadPatientAsync(int patientId)
		{
			throw new NotImplementedException();
		}

		public Task<string> UpdatePatientAsync(Patient patient)
		{
			throw new NotImplementedException();
		}
	}
}
