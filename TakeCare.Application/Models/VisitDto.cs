using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class VisitDto
	{
		[Required]
		public string DoctorEmail { get; set; } = string.Empty;
		[Required]
		public DateTime Slot { get; set; }
		[Required]
		public string PatientEmail { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		public string DoctorFirstName { get; set; } = string.Empty;
		public string DoctorLastName { get; set; } = string.Empty;
		public string DoctorSpecialization { get; set; } = string.Empty;
	}

}
