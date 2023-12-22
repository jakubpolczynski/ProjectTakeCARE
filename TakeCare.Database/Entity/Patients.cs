using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class Patient
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(11)]
		public string Pesel { get; set; } = string.Empty;
		[Required]
		[MaxLength(100)]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		[MaxLength(100)]
		public string LastName { get; set; } = string.Empty;
		[Required]
		[MaxLength(100)]
		public string Email { get; set; } = string.Empty;
		[Required]
		[MaxLength(12)]
		public string Phone { get; set; } = string.Empty;
		[Required]
		[ForeignKey("Address")]
		public int AddressId { get; set; }
		public virtual Address? Address { get; set; }

		public virtual ICollection<Visit>? Visits { get; set; }

		[Required]
		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User? User { get; set; }
	}
}