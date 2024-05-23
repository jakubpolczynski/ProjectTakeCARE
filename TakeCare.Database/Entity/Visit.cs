using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class Visit
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public DateTime Date { get; set; }
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		[ForeignKey("Patient")]
		public int PatientId { get; set; }
		public virtual Patient? Patient { get; set; }
		[Required]
		[ForeignKey("Doctor")]
		public int DoctorId { get; set; }
		public virtual Doctor? Doctor { get; set; }
	}
}