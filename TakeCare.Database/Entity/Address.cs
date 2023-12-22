using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class Address
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string City { get; set; } = string.Empty;
		[Required]
		[MaxLength(200)]
		public string Street { get; set; } = string.Empty;
		[Required]
		[MaxLength(10)]
		public string PostalCode { get; set; } = string.Empty;
	}
}