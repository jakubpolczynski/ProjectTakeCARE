using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class PatientDto
	{
		public int PatientId { get; set; }
		[Required]
		[MinLength(11)]
		[MaxLength(11)]
		public string Pesel { get; set; } = string.Empty;
		[Required]
		[MaxLength(32)]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		[MaxLength(64)]
		public string LastName { get; set; } = string.Empty;
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required]
		[RegularExpression("^[0-9]+$")]
		public string Phone { get; set; } = string.Empty;
		[Required]
		[MaxLength(64)]
		public string City { get; set; } = string.Empty;
		[Required]
		[MaxLength(64)]
		public string Street { get; set; } = string.Empty;
		[Required]
		[RegularExpression("^\\d{2}-\\d{3}$")]
		public string PostalCode { get; set; } = string.Empty;
		[Required]
		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
		public string Password { get; set; } = string.Empty;
		[Required]
		public required string Role { get; set; } = "Patient";

	}
}
