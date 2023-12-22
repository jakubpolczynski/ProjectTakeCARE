using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[EmailAddress]
		[MaxLength(100)]
		public string Email { get; set; } = string.Empty;
		[Required]
		[PasswordPropertyText]
		[MaxLength(1024)]
		public string Password { get; set; } = string.Empty;
		[Required]
		[MaxLength(10)]
		public string Role { get; set; } = string.Empty;
	}
}