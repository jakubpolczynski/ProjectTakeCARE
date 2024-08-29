using TakeCare.Application.Models;

namespace TakeCare.Application.Interfaces
{
	public interface IVisitService
	{
		Task<AvailableDateDto> FindAvailableDate(string? firstName, string? lastName, string specialization, DateTime date);
		Task BookVisit(VisitDto visit);
		Task BookVisitAsReceptionist(VisitDto visit);
		public Task<List<VisitDto>> GetPatientVisits(string patientEmail);
		public Task<List<VisitDto>> GetPatientExecutedVisits(string patientEmail);
		public Task<List<VisitDto>> GetDoctorVisits(string doctorEmail);
		public Task<List<VisitDto>> GetDoctorExecutedVisits(string doctorEmail);
		public Task DeleteBookedVisit(VisitDto visit);
		public Task<VisitDto> GetVisit(int id);

	}
}
