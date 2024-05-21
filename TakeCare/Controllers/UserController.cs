using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Entity;
using TakeCare.Models;

namespace TakeCare.Controllers
{
    [ApiController]
	[Route("/api/[controller]")]
    public class UserController : Controller
    {
		private readonly IGenericService<User>? _userGenericService;
		private readonly IGenericService<Doctor>? _doctorGenericService;
		private readonly IGenericService<Patient>? _patientGenericService;
		private readonly IGenericService<Address>? _addressGenericService;
		private readonly IUserService? _userService;
		private readonly IPatientService? _patientService;

		public UserController(
			IGenericService<User> userGenericService,
			IGenericService<Doctor> doctorGenericService,
			IGenericService<Patient> patientGenericService,
			IGenericService<Address> addressGenericService,
			IUserService userService,
			IPatientService patientService
			)
        {
	        _userGenericService = userGenericService;
	        _doctorGenericService = doctorGenericService;
	        _patientGenericService = patientGenericService;
	        _addressGenericService= addressGenericService;
			_userService = userService;
			_patientService = patientService;

		}
        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor(DoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
				return BadRequest("ModelState is not valid");
            }
            if (doctor.Role != "Doctor" || _userGenericService == null || _doctorGenericService == null)
            {
                return BadRequest("Invalid data:"+ doctor.Role);
            }

            var userEntity = new User
            {
	            Email = doctor.Email,
	            Password = BCrypt.Net.BCrypt.HashPassword(doctor.Password),
	            Role = doctor.Role
            };

			var doctorEntity = new Doctor
            {
				FirstName = doctor.FirstName,
				LastName = doctor.LastName,
				Title = doctor.Title,
				Phone = doctor.Phone,
				Email = doctor.Email,
				User = userEntity
			};

			var userExists = await _userService!.CheckIfUserExistsAsync(userEntity.Email);

			if(userExists)
			{
				return BadRequest("User already exists");
			}
			else
			{
				await _userGenericService.CreateAsync(userEntity);
				await _doctorGenericService.CreateAsync(doctorEntity);
				return Created("Success", doctor);

			}
		}

        [HttpPost("AddPatient")]
		public async Task<IActionResult> AddPatient(PatientDto patient)
        {
	        if (!ModelState.IsValid || patient.Role != "Patient" || _userGenericService == null || _patientGenericService == null || _addressGenericService == null)
	        {
		        return BadRequest();
	        }

	        var userEntity = new User
	        {
		        Email = patient.Email,
	            Password = BCrypt.Net.BCrypt.HashPassword(patient.Password),
				Role = patient.Role
	        };

            var addressEntity = new Address
            {
	            City = patient.City,
	            Street = patient.Street,
	            PostalCode = patient.PostalCode
			};

            var patientEntity = new Patient
            {
	            Pesel = patient.Pesel,
	            FirstName = patient.FirstName,
	            LastName = patient.LastName,
	            Email = patient.Email,
	            Phone = patient.Phone,
	            Address = addressEntity,
	            User = userEntity
            };

			var userExists = await _userService!.CheckIfUserExistsAsync(userEntity.Email);
			var patientExists = await _patientService!.CheckIfPatientExistsAsync(patient.Pesel);

			if (userExists || patientExists)
			{
				return BadRequest("You already have an account");
			}
			else
			{
				await _userGenericService.CreateAsync(userEntity);
				await _addressGenericService.CreateAsync(addressEntity);
				await _patientGenericService.CreateAsync(patientEntity);

				return Created("Success", patient);
			}

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
	        if (!ModelState.IsValid || _userGenericService == null)
	        {
				return BadRequest();
			}

			await _userGenericService.DeleteAsync(id);

			return Ok();
		}

        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
	        if (!ModelState.IsValid || _userGenericService == null)
	        {
				return BadRequest();
			}

			var user = await _userGenericService.ReadAsync(id);



			return Ok(user);
		}

        [HttpPost("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(PatientDto patient)
        {
			
	        if (!ModelState.IsValid || patient.Role != "Patient" || _userGenericService == null || _patientGenericService == null || _addressGenericService == null)
			{
				return BadRequest();
			}

			var userEntity = new User
	        {
		        Email = patient.Email,
		        Password = patient.Password,
		        Role = patient.Role
	        };

	        var addressEntity = new Address
	        {
		        City = patient.City,
		        Street = patient.Street,
		        PostalCode = patient.PostalCode
	        };

	        var patientEntity = new Patient
	        {
		        Pesel = patient.Pesel,
		        FirstName = patient.FirstName,
		        LastName = patient.LastName,
		        Email = patient.Email,
		        Phone = patient.Phone,
		        Address = addressEntity,
		        User = userEntity
	        };

			await _userGenericService.UpdateAsync(userEntity);
			await _addressGenericService.UpdateAsync(addressEntity);
			await _patientGenericService.UpdateAsync(patientEntity);

			return Ok();
        }

        [HttpPost("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctor(DoctorDto doctor)
        {
	        if (!ModelState.IsValid || doctor.Role != "Doctor" || _userGenericService == null || _doctorGenericService == null)
	        {
		        return BadRequest();
	        }

	        var userEntity = new User
	        {
		        Email = doctor.Email,
		        Password = doctor.Password,
		        Role = doctor.Role
	        };

	        var doctorEntity = new Doctor
	        {
		        FirstName = doctor.FirstName,
		        LastName = doctor.LastName,
		        Title = doctor.Title,
		        Phone = doctor.Phone,
		        Email = doctor.Email,
		        User = userEntity
	        };

	        await _userGenericService.UpdateAsync(userEntity);
	        await _doctorGenericService.UpdateAsync(doctorEntity);

	        return Ok();
        }
	}
}
