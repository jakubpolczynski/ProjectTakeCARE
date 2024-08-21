using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TakeCare.Application.Models;

namespace TakeCare.Application.Interfaces
{
	public interface IEmailService
	{
		Task SendEmail(ExaminationDto examination, IFormFile file);
	}
}
