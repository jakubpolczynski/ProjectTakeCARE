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
        private readonly IUserService? _userService;
        private readonly IDoctorService? _doctorService;
        private readonly IPatientService? _patientService;
        private readonly IAddressService? _addressService;

        public UserController(
	        IUserService userService, 
	        IDoctorService doctorService,
	        IPatientService patientService,
			IAddressService addressService
			)
        {
            _userService = userService;
            _doctorService = doctorService;
			_patientService = patientService;
			_addressService = addressService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(DoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (_userService == null || doctor.Role != "Doctor" || _doctorService == null)
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

            await _userService.CreateUserAsync(userEntity);
            await _doctorService.CreateDoctorAsync(doctorEntity);

			return Ok();
        }

        [HttpPost]
		public async Task<IActionResult> AddUser(PatientDto patient)
        {
	        if (!ModelState.IsValid)
	        {
		        return BadRequest();
			}
	        if (_userService == null || patient.Role != "Patient" || _patientService == null || _addressService == null)
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

	        await _userService.CreateUserAsync(userEntity);
	        await _addressService.CreateAddressAsync(addressEntity);
	        await _patientService.CreatePatientAsync(patientEntity);

	        return Ok();
        }
	}
}
