using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeCare.Database.Entity
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_id { get; set; }
        [Required]
        public string User_email { get; set; } = string.Empty;
        [Required]
        public string User_password { get; set;} = string.Empty;
        [Required]
        public string User_Role { get; set; } = string.Empty;
    }
}
