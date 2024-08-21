using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class ExaminationDto
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public DateTime? Date { get; set; }
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		public string Type { get; set; } = string.Empty;
		[Required]
		public string Result { get; set; } = string.Empty;
		[Required]
		public string PatientEmail { get; set; } = string.Empty;
		[Required]
		public string DoctorEmail { get; set; } = string.Empty;
		[Required]
		public int VisitId { get; set; }
	}
}
