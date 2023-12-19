using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class Address
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Address_id { get; set; }
		[Required]
		public string Address_city { get; set; } = string.Empty;
		[Required]
		public string Address_street { get; set; } = string.Empty;
		[Required]
		public string Address_postal_code { get; set; } = string.Empty;
	}
}