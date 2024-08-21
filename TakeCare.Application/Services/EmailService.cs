using System.Buffers.Text;
using Microsoft.AspNetCore.Http;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace TakeCare.Application.Services
{
	public class EmailService : IEmailService
	{
		private readonly string _secureKey;
		private readonly string _publicKey;
		public EmailService(IConfiguration configuration)
		{
			_secureKey = configuration!.GetSection("Email:Secret").Value!;
			_publicKey = configuration!.GetSection("Email:Public").Value!;
		}

		public async Task SendEmail(ExaminationDto examination, IFormFile file)
		{
			
			var client = new MailjetClient(_publicKey, _secureKey);
			var request = new MailjetRequest
				{
					Resource = Send.Resource,
				}
				.Property(Send.FromEmail, examination.DoctorEmail)
				.Property(Send.Subject, examination.Name)
				.Property(Send.TextPart, $"{examination.Description}\n\n{examination.Result}")
				.Property(Send.Recipients, new JArray {
					new JObject {
						{"Email", examination.PatientEmail}
					}
				}).Property(Send.Attachments, new JArray
				{
					new JObject {
						{"Content-type", "text/plain"},
						{"Filename", $"Examination_{examination.PatientEmail}_{examination.Date}"},
						{"content", await FormatPdf(file)}
					}
				});
			var response = await client.PostAsync(request);
			if (response.IsSuccessStatusCode)
			{
				Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
				Console.WriteLine(response.GetData());
			}
			else
			{
				Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
				Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
				Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
			}
		}

		private static async Task<string> FormatPdf(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				throw new ArgumentException("File cannot be null or empty.", nameof(file));
			}

			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);
				var base64String = Convert.ToBase64String(memoryStream.ToArray());
				return base64String;
			}
		}

	}
}
