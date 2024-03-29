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
		private readonly IGenericService<User>? _userService;
		private readonly IGenericService<Doctor>? _doctorService;
		private readonly IGenericService<Patient>? _patientService;
		private readonly IGenericService<Address>? _addressService;

		public UserController(
			IGenericService<User> userService,
			IGenericService<Doctor> doctorService,
			IGenericService<Patient> patientService,
			IGenericService<Address> addressService
			)
        {
	        _userService = userService;
	        _doctorService = doctorService;
	        _patientService = patientService;
	        _addressService= addressService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(DoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (doctor.Role != "Doctor" || _userService == null || _doctorService == null)
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

            await _userService.CreateAsync(userEntity);
            await _doctorService.CreateAsync(doctorEntity);

			return Ok();
        }

        [HttpPost]
		public async Task<IActionResult> AddUser(PatientDto patient)
        {
	        if (!ModelState.IsValid || patient.Role != "Patient" || _userService == null || _patientService == null || _addressService == null)
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

	        await _userService.CreateAsync(userEntity);
	        await _addressService.CreateAsync(addressEntity);
	        await _patientService.CreateAsync(patientEntity);

	        return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
	        if (!ModelState.IsValid || _userService == null)
	        {
				return BadRequest();
			}

			await _userService.DeleteAsync(id);

			return Ok();
		}

        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
	        if (!ModelState.IsValid || _userService == null)
	        {
				return BadRequest();
			}

			var user = await _userService.ReadAsync(id);

			return Ok(user);
		}

        [HttpPost]
        public async Task<IActionResult> UpdateUser(PatientDto patient)
        {
			
	        if (!ModelState.IsValid || patient.Role != "Patient" || _userService == null || _patientService == null || _addressService == null)
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

			await _userService.UpdateAsync(userEntity);
			await _addressService.UpdateAsync(addressEntity);
			await _patientService.UpdateAsync(patientEntity);

			return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(DoctorDto doctor)
        {
	        if (!ModelState.IsValid || doctor.Role != "Doctor" || _userService == null || _doctorService == null)
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

	        await _userService.UpdateAsync(userEntity);
	        await _doctorService.UpdateAsync(doctorEntity);

	        return Ok();
        }
	}
}
