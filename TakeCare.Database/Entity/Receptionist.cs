using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class Receptionist
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		public string LastName { get; set; } = string.Empty;
		[Required]
		public string Email {  get; set; } = string.Empty;
		[Required]
		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User? User { get; set; }
	}
}
