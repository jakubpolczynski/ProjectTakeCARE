using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class ExaminationImage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string ImagePath { get; set; } = string.Empty;

		[Required]
		[ForeignKey("Examination")]
		public int ExaminationId { get; set; }

		public virtual Examination? Examination { get; set; }
	}
}
