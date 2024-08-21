using TakeCare.Application.Models;

namespace TakeCare.Application.Interfaces
{
	public interface IExaminationService
	{
		Task<ExaminationDto[]> GetPatientExaminations(string patientEmail);
		Task<ExaminationDto[]> GetDoctorExaminations(string doctorEmail);
		Task AddExamination(ExaminationDto examinationDto);
	}
}
