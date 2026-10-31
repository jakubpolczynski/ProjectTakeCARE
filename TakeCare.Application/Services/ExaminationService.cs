using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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

		public async Task<ExaminationDto[]> GetPatientExaminations(int patientUserId)
		{
			if (patientUserId <= 0)
			{
				throw new ArgumentException("Invalid patient specified.");
			}

			try
			{
				var examinations = await _dbSetExamination
					.Where(e => e.Patient != null && e.Patient.UserId == patientUserId)
					.Select(e => new ExaminationDto
					{
						Id = e.Id,
						Date = e.Date,
						Name = e.Name,
						Description = e.Description,
						Type = e.Type,
						Result = e.Result,
						PatientEmail = e.Patient != null ? e.Patient.Email : string.Empty,
						DoctorEmail = e.Doctor != null ? e.Doctor.Email : string.Empty,
						VisitId = e.Visit != null ? e.Visit.Id : 0,
						Images = e.Images != null ? e.Images.Select(i => i.ImagePath).ToList() : new List<string>()
					})
					.ToArrayAsync();

				return examinations;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Examinations not found.", ex);
			}
		}


		public async Task<ExaminationDto[]> GetDoctorExaminations(int doctorUserId)
		{
			if (doctorUserId <= 0)
			{
				throw new ArgumentException("Invalid doctor specified.");
			}

			try
			{
				var examinations = await _dbSetExamination
					.Where(e => e.Doctor != null && e.Doctor.UserId == doctorUserId)
					.Select(e => new ExaminationDto
					{
						Id = e.Id,
						Date = e.Date,
						Name = e.Name,
						Description = e.Description,
						Type = e.Type,
						Result = e.Result,
						PatientEmail = e.Patient != null ? e.Patient.Email : string.Empty,
						DoctorEmail = e.Doctor != null ? e.Doctor.Email : string.Empty,
						VisitId = e.Visit != null ? e.Visit.Id : 0,
						Images = e.Images != null ? e.Images.Select(i => i.ImagePath).ToList() : new List<string>()
					})
					.ToArrayAsync();

				return examinations;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Examinations not found.", ex);
			}
		}

		public async Task<int> AddExamination(ExaminationDto examinationDto, int doctorUserId, byte[] pdfContent)
		{
			if (pdfContent is null || pdfContent.Length < 5 || pdfContent.Length > 5 * 1024 * 1024 ||
				!pdfContent.AsSpan(0, 5).SequenceEqual("%PDF-"u8))
				throw new ArgumentException("Invalid PDF document.");
			if (examinationDto == null)
			{
				throw new ArgumentException("Invalid examination specified.");
			}

			try
			{
                var visit = await _dbSetVisit.Include(v => v.Patient).Include(v => v.Doctor)
                    .SingleOrDefaultAsync(v => v.Id == examinationDto.VisitId &&
                        v.Doctor!.UserId == doctorUserId && !v.IsVisitExecuted);
                if (visit?.Patient == null || visit.Doctor == null ||
                    visit.Patient.Email != examinationDto.PatientEmail || visit.Doctor.Email != examinationDto.DoctorEmail)
                    throw new VisitUnavailableException();
                var patient = visit.Patient;
                var doctor = visit.Doctor;

				var examination = new Examination
				{
					Date = examinationDto.Date,
					Name = examinationDto.Name,
					Description = examinationDto.Description,
					Type = examinationDto.Type,
					Result = examinationDto.Result,
					PdfContent = pdfContent,
					Patient = patient,
					Doctor = doctor,
					Visit = visit
				};

				if (examinationDto.Images != null && examinationDto.Images.Count > 0)
				{
					foreach (var imagePath in examinationDto.Images)
					{
						var examinationImage = new ExaminationImage
						{
							ImagePath = imagePath,
							Examination = examination
						};
						examination.Images.Add(examinationImage);
					}
				}

				_dbSetExamination.Add(examination);

				visit.IsVisitExecuted = true;

				await _context.SaveChangesAsync();
				return examination.Id;
			}
			catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
			{
				throw new VisitUnavailableException();
			}
			catch (VisitUnavailableException) { throw; }
			catch (Exception ex)
			{
				throw new InvalidOperationException("Examination not added.", ex);
			}
		}

	}
}
