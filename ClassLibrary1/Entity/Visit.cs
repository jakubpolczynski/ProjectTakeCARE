using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
    public class Visit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Visit_id { get; set; }
        [Required]
        [ForeignKey("Patient")]
        public int Patient_id { get; set; }
        public virtual Patient? Patient { get; set; }
        [Required]
        [ForeignKey("Doctor")]
        public int Doctor_id { get; set; }
        public virtual Doctor? Doctor { get; set; }
        [Required]
        public DateTime Visit_date { get; set; }
        [Required]
        public string Visit_description { get; set; } = string.Empty;
    }
}
