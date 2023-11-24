using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
    public class Examination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Examination_id { get; set; }
        [Required]
        public DateTime? Examination_date { get; set;}
        [Required]
        public string Examination_name { get; set;} = string.Empty;
        [Required]
        public string Examination_description { get; set;} = string.Empty;
        [Required]
        public string Examination_type { get; set; } = string.Empty;
        [Required]
        public string Examination_result { get; set;} = string.Empty;
        [Required]
        [ForeignKey("Patient")]
        public int Patient_id { get; set;}
        public virtual Patient? Patient { get; set;}
        [Required]
        [ForeignKey("Doctor")]
        public int Doctor_id { get; set;}
        public virtual Doctor? Doctor { get; set;}
    }
}
