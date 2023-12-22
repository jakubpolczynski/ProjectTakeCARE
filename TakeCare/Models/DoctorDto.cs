
namespace TakeCare.Models
{
	public class DoctorDto
	{
		public int DoctorId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public required string Role { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}
}
