using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Patient_id { get; set; }
        [Required]
        public string Patient_pesel { get; set; } = string.Empty;
        [Required]
        public string Patient_first_name { get; set; } = string.Empty;
        [Required]
        public string Patient_last_name { get; set;} = string.Empty;
        [Required]
        public string Patient_email { get; set; } = string.Empty;
        [Required]
        public string Patient_phone { get; set; } = string.Empty;
        [Required]
        [ForeignKey("Address")]
        public int Address_id { get; set; }
        public virtual Address? Address { get; set; }
        public virtual ICollection<Visit>? Visits { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
