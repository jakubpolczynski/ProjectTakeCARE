using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TakeCare.Application.Interfaces;
using TakeCare.Application.Models;

namespace TakeCare.Application.Services;

public sealed class EmailService(IConfiguration configuration, HttpClient client) : IEmailService
{
    public async Task SendEmail(ExaminationDto examination, IFormFile file)
    {
        if (!configuration.GetValue<bool>("Email:Enabled")) return;
        if (file.Length is <= 0 or > 5 * 1024 * 1024)
            throw new ArgumentException("Invalid attachment.");
        using var data = new MemoryStream();
        await file.CopyToAsync(data);
        var message = new
        {
            Messages = new[]
            {
                new
                {
                    From = new { Email = configuration["Email:FromEmail"], Name = "TakeCARE" },
                    To = new[] { new { Email = examination.PatientEmail } },
                    Subject = "TakeCARE examination",
                    TextPart = examination.Description + "\n\n" + examination.Result,
                    Attachments = new[]
                    {
                        new { ContentType = "application/pdf", Filename = "examination.pdf",
                            Base64Content = Convert.ToBase64String(data.ToArray()) }
                    }
                }
            }
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailjet.com/v3.1/send");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(configuration["Email:Public"] + ":" + configuration["Email:Secret"])));
        request.Content = JsonContent.Create(message);
        using var response = await client.SendAsync(request);
        // Provider responses can contain recipient addresses, message text and diagnostic information.
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException("Email provider rejected the request.");
    }
}
