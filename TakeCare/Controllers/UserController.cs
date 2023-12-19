using Microsoft.AspNetCore.Mvc;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Entity;

namespace TakeCare.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserDbService? _userService;
        
        public UserController(IUserDbService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (_userService == null)
            {
                return BadRequest();
            }

            await _userService.AddUserAsync(user);
            return Ok();
        }
    }
}
