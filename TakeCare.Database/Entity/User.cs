using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TakeCare.Database.Entity
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required]
		[PasswordPropertyText]
		[JsonIgnore]
		public string Password { get; set; } = string.Empty;
		[Required]
		public string Role { get; set; } = string.Empty;
	}
}