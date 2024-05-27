using System.ComponentModel.DataAnnotations;

namespace TakeCare.Application.Models
{
	public class FindDateDto
	{
		[MaxLength(32)]
		public string? FirstName { get; set; } = string.Empty;
		[MaxLength(64)]
		public string? LastName { get; set; } = string.Empty;
		[Required]
		public string Specialization { get; set; } = string.Empty;
		[Required]
		public DateTime Date { get; set; }
	}
}
