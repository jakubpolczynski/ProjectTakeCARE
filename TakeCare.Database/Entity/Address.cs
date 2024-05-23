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
		public string City { get; set; } = string.Empty;
		[Required]
		public string Street { get; set; } = string.Empty;
		[Required]
		public string PostalCode { get; set; } = string.Empty;
	}
}