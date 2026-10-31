using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;
using Xunit;

namespace TakeCare.SecurityTests;

public sealed class PdfStorageTests
{
    private static readonly byte[] Pdf = Encoding.ASCII.GetBytes("%PDF-1.4\n%%EOF\n");

    [Fact]
    public void SqlServer_migration_is_discoverable_and_generates_pdf_column_without_connecting()
    {
        var options = new DbContextOptionsBuilder<TakeCareDbContext>()
            .UseSqlServer("Server=localhost;Database=Unused;Integrated Security=true;Encrypt=true").Options;
        using var db = new TakeCareDbContext(options);
        var migration = "20261031193000_StoreExaminationPdf";
        Assert.Contains(migration, db.Database.GetMigrations());
        var sql = db.GetService<IMigrator>().GenerateScript("20240829085527_AddedReceptionistTable", migration);
        Assert.Contains("[PdfContent]", sql);
        Assert.Contains("varbinary(max)", sql);
        Assert.Contains("NULL", sql);
        Assert.Contains("COUNT_BIG(*) > 1", sql);
        Assert.Contains("CREATE UNIQUE INDEX [IX_ExaminationTable_VisitId]", sql);
    }

    private static async Task NewVisit(ApiFactory factory)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
        db.VisitTable!.Add(new Visit { Id = 2, PatientId = 1, DoctorId = 1,
            Date = DateTime.UtcNow.AddDays(1), Description = "test visit" });
        await db.SaveChangesAsync();
    }

    private static MultipartFormDataContent Form(byte[] content)
    {
        var form = new MultipartFormDataContent();
        foreach (var (key, value) in new Dictionary<string, string>
        {
            ["Id"] = "0", ["VisitId"] = "2", ["Date"] = "2026-11-01T12:00:00Z",
            ["Name"] = "test", ["Type"] = "test", ["Description"] = "test",
            ["Result"] = "test", ["DoctorEmail"] = "doctor@example.invalid",
            ["PatientEmail"] = "one@example.invalid"
        }) form.Add(new StringContent(value), key);
        var file = new ByteArrayContent(content);
        file.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        form.Add(file, "file", "result.pdf");
        return form;
    }

    [Fact]
    public async Task Created_pdf_is_saved_with_examination_and_downloaded_by_patient()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        await NewVisit(factory);
        using var doctor = factory.Client();
        doctor.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(3, "Doctor", "doctor@example.invalid"));
        await ApiFactory.Csrf(doctor);
        using var form = Form(Pdf);
        var response = await doctor.PostAsync("/api/Examination/AddExamination", form);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("disabled", result.GetProperty("emailDelivery").GetString());
        var id = result.GetProperty("id").GetInt32();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
            Assert.Equal(Pdf, (await db.ExaminationTable!.SingleAsync(e => e.Id == id)).PdfContent);
            Assert.True((await db.VisitTable!.SingleAsync(v => v.Id == 2)).IsVisitExecuted);
        }
        using var patient = factory.Client();
        patient.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(1, "Patient", "one@example.invalid"));
        var download = await patient.GetAsync("/api/Examination/" + id + "/pdf");
        Assert.Equal(HttpStatusCode.OK, download.StatusCode);
        Assert.Equal("application/pdf", download.Content.Headers.ContentType!.MediaType);
        Assert.Equal("attachment", download.Content.Headers.ContentDisposition!.DispositionType);
        Assert.Equal("no-store", download.Headers.CacheControl!.ToString());
        Assert.Equal(Pdf, await download.Content.ReadAsByteArrayAsync());
        var ownerDownload = await doctor.GetAsync("/api/Examination/" + id + "/pdf");
        Assert.Equal(HttpStatusCode.OK, ownerDownload.StatusCode);
    }

    [Fact]
    public async Task Pdf_cannot_be_accessed_by_another_patient_or_receptionist_or_anonymous_user()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
            (await db.ExaminationTable!.SingleAsync(e => e.Id == 1)).PdfContent = Pdf;
            await db.SaveChangesAsync();
        }
        using var client = factory.Client();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/Examination/1/pdf")).StatusCode);
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(2, "Patient", "two@example.invalid"));
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/api/Examination/1/pdf")).StatusCode);
        client.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(4, "Receptionist", "reception@example.invalid"));
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/Examination/1/pdf")).StatusCode);
    }

    [Fact]
    public async Task Invalid_pdf_is_rejected_without_creating_examination_or_completing_visit()
    {
        await using var factory = new ApiFactory();
        await factory.Seed();
        await NewVisit(factory);
        using var doctor = factory.Client();
        doctor.DefaultRequestHeaders.Authorization = new("Bearer", factory.Token(3, "Doctor", "doctor@example.invalid"));
        await ApiFactory.Csrf(doctor);
        using var form = Form(Encoding.ASCII.GetBytes("not a pdf"));
        Assert.Equal(HttpStatusCode.BadRequest, (await doctor.PostAsync("/api/Examination/AddExamination", form)).StatusCode);
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TakeCareDbContext>();
        Assert.False((await db.VisitTable!.SingleAsync(v => v.Id == 2)).IsVisitExecuted);
        Assert.False(await db.ExaminationTable!.AnyAsync(e => e.VisitId == 2));
    }
}
