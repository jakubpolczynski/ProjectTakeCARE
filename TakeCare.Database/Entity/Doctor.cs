﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeCare.Database.Entity
{
	public class Doctor
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		public string LastName { get; set; } = string.Empty;
		[Required]
		public string Specialization { get; set; } = string.Empty;
		[Required]
		public string Phone { get; set; } = string.Empty;
		[Required]
		public string Email { get; set; } = string.Empty;
		public virtual ICollection<Visit>? Visits { get; set; }
		[Required]
		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User? User { get; set; }
	}
}