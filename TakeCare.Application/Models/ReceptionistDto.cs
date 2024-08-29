using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class ReceptionistDto
	{
		public int ReceptionistId { get; set; }
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
		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
		public string Password { get; set; } = string.Empty;
		[Required]
		public required string Role { get; set; } = "Receptionist";
	}
}
