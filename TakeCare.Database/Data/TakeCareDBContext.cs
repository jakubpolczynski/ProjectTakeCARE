using TakeCare.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace TakeCare.Database.Data
{
	public class TakeCareDBContext : DbContext
	{
		public TakeCareDBContext(DbContextOptions<TakeCareDBContext> options) : base(options)
		{
		}

		public DbSet<Address> AddressTable { get; set; }
		public DbSet<Doctor> DoctorTable { get; set; }
		public DbSet<Examination> ExaminationTable { get; set; }
		public DbSet<Patient> PatientTable { get; set; }
		public DbSet<Visit> VisitTable { get; set; }
		public DbSet<User> UserTable { get; set; }
	}
}