using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Doctor_id { get; set; }
        [Required]
        public string Doctor_first_name { get; set; } = string.Empty;
        [Required]
        public string Doctor_last_name { get; set;} = string.Empty;
        [Required]
        public string Doctor_title { get; set; } = string.Empty;
        [Required]
        public string Doctor_phone { get; set; } = string.Empty;
        [Required]
        public string Doctor_email { get; set; } = string.Empty;
        public virtual ICollection<Visit>? Visits { get; set; }
        [Required]
        [ForeignKey("User")]
        public int User_id { get; set; }
        public virtual User? User { get; set; }
    }
}
