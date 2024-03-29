using Microsoft.AspNetCore.Mvc;

namespace TakeCare.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IConfiguration? _config;
		public LoginController(IConfiguration config)
		{
			_config = config;
		}
	}
}
