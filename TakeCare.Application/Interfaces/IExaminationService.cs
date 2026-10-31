using TakeCare.Application.Models;

namespace TakeCare.Application.Interfaces
{
	public interface IExaminationService
	{
		Task<ExaminationDto[]> GetPatientExaminations(int patientUserId);
		Task<ExaminationDto[]> GetDoctorExaminations(int doctorUserId);
		Task<int> AddExamination(ExaminationDto examinationDto, int doctorUserId, byte[] pdfContent);
	}
}
