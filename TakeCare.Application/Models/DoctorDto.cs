using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class DoctorDto
	{
		public int? DoctorId { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required]
		[MinLength(8)]
		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
		public string Password { get; set; } = string.Empty;
		[Required]
		public required string Role { get; set; } = "Doctor";
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
	}
}
