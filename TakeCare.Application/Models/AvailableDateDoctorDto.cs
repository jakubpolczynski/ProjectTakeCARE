using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class AvailableDateDoctorDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required]
		[MaxLength(32)]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		[MaxLength(64)]
		public string LastName { get; set; } = string.Empty;
		[Required]
		[MaxLength(128)]
		public string Specialization { get; set; } = string.Empty;
		[Required]
		[RegularExpression("^[0-9]+$")]
		public string Phone { get; set; } = string.Empty;
		[Required]
		public List<DateTime> AvailableSlots { get; set; } = new List<DateTime>();
	}
}
