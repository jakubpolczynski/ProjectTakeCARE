using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int User_id { get; set; }
		[Required]
		public string User_email { get; set; } = string.Empty;
		[Required]
		public string User_password { get; set; } = string.Empty;
		[Required]
		public string User_Role { get; set; } = string.Empty;
	}
}