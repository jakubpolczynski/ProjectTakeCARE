namespace TakeCare.Models
{
	public class PatientDto
	{
		public int PatientId { get; set; }
		public string Pesel { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string Street { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public required string Role { get; set; }

	}
}
