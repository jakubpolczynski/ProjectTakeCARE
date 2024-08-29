using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class VisitDto
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string DoctorEmail { get; set; } = string.Empty;
		[Required]
		public DateTime Slot { get; set; }
		[Required]
		public string PatientEmail { get; set; } = string.Empty;
		[Required]
		public string Reason { get; set; } = string.Empty;
		[Required]
		public bool IsVisitExecuted { get; set; } = false;
		public string DoctorFirstName { get; set; } = string.Empty;
		public string DoctorLastName { get; set; } = string.Empty;
		public string DoctorSpecialization { get; set; } = string.Empty;
		public string PatientFirstName { get; set; } = string.Empty;
		public string PatientLastName { get; set; } = string.Empty;
		public string PatientPesel { get; set; } = string.Empty;
	}

}
