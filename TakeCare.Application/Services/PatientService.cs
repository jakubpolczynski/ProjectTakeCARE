using TakeCare.Application.Interfaces;
using TakeCare.Database.Entity;
using Couchbase.Core.Exceptions;
using TakeCare.Database.Data;

namespace TakeCare.Application.Services
{
	public class PatientService : IPatientService
	{
		private readonly TakeCareDbContext _context;

		public PatientService(TakeCareDbContext context)
		{
			_context = context ?? throw new InvalidArgumentException("Invalid context");
		}
		public async Task<bool> CheckIfPatientExistAsync(int patientId)
		{
            // Check if patient id is valid
            if (patientId < 1)
            {
                throw new InvalidArgumentException("Invalid patient id");
            }

            // Check if patient exists
            var patient = await _context.PatientTable.FindAsync(patientId);

            // Return true if patient exists
            return patient != null;
        }

		public async Task<string> CreatePatientAsync(Patient patient)
		{
			// Check if patient is null
			if (patient == null) throw new InvalidArgumentException("Invalid patient provided");

			// Check if patient already exists
			var patientExists = await ReadPatientAsync(patient.Id);
			if (patientExists == patient) 
			{
				throw new InvalidArgumentException("Patient already exists");
			}

			// Add patient to database
			await _context.PatientTable.AddAsync(patient);

			// Save changes
			var patientAdded = await _context.SaveChangesAsync();

			// Chec if patient was added
			if(patientAdded == 1)
			{
				return "Patient added";
			}
			throw new InvalidArgumentException("Patient not added");
		}

		public Task<string> DeletePatientAsync(int patientId)
		{
			throw new NotImplementedException();
		}

		public async Task<Patient> ReadPatientAsync(int patientId)
		{
			// Check if patient id is valid
			if(patientId < 1)
			{
				throw new InvalidArgumentException("Invalid patient id");
			}

			// Read patient
			var patient = await _context.PatientTable.FindAsync(patientId);

			// Return patient
			return patient ?? throw new InvalidArgumentException("Patient not found");
		}

		public Task<string> UpdatePatientAsync(Patient patient)
		{
			throw new NotImplementedException();
		}
	}
}
