using System.ComponentModel.DataAnnotations;

namespace TakeCare.Models
{
	public class FindDateDto
	{
		[Required]
		[MaxLength(32)]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		[MaxLength(64)]
		public string LastName { get; set; } = string.Empty;
		[Required]
		public DateTime Date { get; set; }
	}
}
