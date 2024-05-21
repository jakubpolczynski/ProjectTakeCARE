using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Entity;
using TakeCare.Models;

namespace TakeCare.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : Controller
	{
		private readonly IGenericService<User>? _userGenericService;
		private readonly IUserService? _userService;

		public LoginController(
			IGenericService<User> userGenericService, 
			IUserService userService
			)
		{
			_userGenericService = userGenericService;
			_userService = userService;
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDto login)
		{
			if(login.Email == null || login.Password == null)
			{
				return BadRequest("Invalid data");
			}

			var user = await _userService!.GetUserByEmailAsync(login.Email);

			if(user == null)
			{
				return BadRequest("Invalid credentials");
			}

			if(!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
			{
				return BadRequest("Invalid credentials");
			}

			return Ok(user);


		}
	}
}
