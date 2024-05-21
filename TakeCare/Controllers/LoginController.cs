using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Services;
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
		private readonly JwtService? _jwtService;

		public LoginController(
			IGenericService<User> userGenericService, 
			IUserService userService,
			JwtService jwtService
			)
		{
			_userGenericService = userGenericService;
			_userService = userService;
			_jwtService = jwtService;
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

			var jwt = _jwtService!.Generate(user.Id, user.Role);

			Response.Cookies.Append("jwt", jwt, new CookieOptions
			{
				HttpOnly = true
			});

			return Ok("Success login");
		}

		[HttpGet("GetUser")]
		public async Task<IActionResult> GetUser() {
			try
			{
				var jwt = Request.Cookies["jwt"];

				var token = _jwtService!.Verify(jwt);

				var userId = int.Parse(_jwtService.DecryptString(token.Issuer.ToString()));

				var user = await _userGenericService!.ReadAsync(userId);
			
				return Ok(user);
			} 
			catch
			{
				return Unauthorized();
			}
		}
	}
}
