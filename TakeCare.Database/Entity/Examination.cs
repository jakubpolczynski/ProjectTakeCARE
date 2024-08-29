using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class Examination
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public DateTime? Date { get; set; }
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		public string Type { get; set; } = string.Empty;
		[Required]
		public string Result { get; set; } = string.Empty;
		public virtual ICollection<ExaminationImage> Images { get; set; } = new List<ExaminationImage>();
		[Required]
		[ForeignKey("Patient")]
		public int PatientId { get; set; }
		public virtual Patient? Patient { get; set; }
		[Required]
		[ForeignKey("Doctor")]
		public int DoctorId { get; set; }
		public virtual Doctor? Doctor { get; set; }
		[ForeignKey("Visit")]
		public int VisitId { get; set; }
		public virtual Visit? Visit { get; set; }
	}
}