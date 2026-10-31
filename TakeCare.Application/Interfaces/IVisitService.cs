using TakeCare.Application.Models;

namespace TakeCare.Application.Interfaces
{
	public interface IVisitService
	{
		Task<AvailableDateDto> FindAvailableDate(string? firstName, string? lastName, string specialization, DateTime date);
		Task BookVisit(VisitDto visit, int patientUserId);
		Task BookVisitAsReceptionist(VisitDto visit);
		public Task<List<VisitDto>> GetPatientVisits(int patientUserId);
		public Task<List<VisitDto>> GetPatientExecutedVisits(int patientUserId);
		public Task<List<VisitDto>> GetDoctorVisits(int doctorUserId);
		public Task<List<VisitDto>> GetDoctorExecutedVisits(int doctorUserId);
		public Task<bool> DeleteBookedVisit(VisitDto visit, int userId, string role);
		public Task<VisitDto> GetVisit(int id);

	}
}
