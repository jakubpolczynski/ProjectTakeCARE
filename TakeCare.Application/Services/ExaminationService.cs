using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;

namespace TakeCare.Application.Services
{
	public class ExaminationService : IExaminationService
	{
		private readonly TakeCareDbContext _context;
		private readonly DbSet<Examination> _dbSetExamination;
		private readonly DbSet<Patient> _dbSetPatient;
		private readonly DbSet<Doctor> _dbSetDoctor;
		private readonly DbSet<Visit> _dbSetVisit;

		public ExaminationService(TakeCareDbContext context)
		{
			_context = context;
			_dbSetExamination = _context.Set<Examination>();
			_dbSetDoctor = _context.Set<Doctor>();
			_dbSetPatient = _context.Set<Patient>();
			_dbSetVisit = _context.Set<Visit>();
		}

		public async Task<ExaminationDto[]> GetPatientExaminations(string patientEmail)
		{
			if(patientEmail == null)
			{
				throw new ArgumentException("Invalid patient specified.");
			}
			try
			{
				var examinations = await _dbSetExamination
					.Where(e => e.Patient!.Email == patientEmail)
					.Select(e => new ExaminationDto { 
						Id = e.Id,
						Date = e.Date,
						Name = e.Name,
						Description = e.Description,
						Type = e.Type,
						Result = e.Result,
						PatientEmail = e.Patient!.Email,
						DoctorEmail	= e.Doctor!.Email,
						VisitId = e.Visit!.Id
					})
					.ToArrayAsync();

				return examinations;
			} 
			catch(Exception e)
			{
				throw new InvalidOperationException("Examinations not found.");
			}
		}

		public async Task<ExaminationDto[]> GetDoctorExaminations(string doctorEmail)
		{
			if (doctorEmail == null)
			{
				throw new ArgumentException("Invalid doctor specified.");
			}
			try
			{

				var examinations = await _dbSetExamination
					.Where(e => e.Doctor!.Email == doctorEmail)
					.Select(e => new ExaminationDto
					{
						Id = e.Id,
						Date = e.Date,
						Name = e.Name,
						Description = e.Description,
						Type = e.Type,
						Result = e.Result,
						PatientEmail = e.Patient!.Email,
						DoctorEmail = e.Doctor!.Email,
						VisitId = e.Visit!.Id
					})
					.ToArrayAsync();

				return examinations;
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("Examinations not found.");
			}
		}

		public async Task AddExamination(ExaminationDto examinationDto)
		{
			if(examinationDto == null)
			{
				throw new ArgumentException("Invalid examination specified.");
			}
			try
			{
				var patient = await _dbSetPatient.FirstOrDefaultAsync(p => p.Email == examinationDto.PatientEmail);
				var doctor = await _dbSetDoctor.FirstOrDefaultAsync(u => u.Email == examinationDto.DoctorEmail);
				var visit = await _dbSetVisit.FirstOrDefaultAsync(v => v.Id == examinationDto.VisitId);

				if (patient == null || doctor == null || visit == null)
				{
					throw new Exception("Patient, Doctor, or Visit not found");
				}

				var examination = new Examination
				{
					Date = examinationDto.Date,
					Name = examinationDto.Name,
					Description = examinationDto.Description,
					Type = examinationDto.Type,
					Result = examinationDto.Result,
					Patient = patient,
					Doctor = doctor,
					Visit = visit
				};

				_dbSetExamination.Add(examination);

				visit.IsVisitExecuted = true;

				await _context.SaveChangesAsync();
			}
			catch(Exception e)
			{
				throw new InvalidOperationException("Examination not added.");
			}
		}

	}
}
