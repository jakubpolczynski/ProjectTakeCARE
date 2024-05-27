using TakeCare.Application.Models;

namespace TakeCare.Application.Interfaces
{
	public interface IVisitService
	{
		Task<AvailableDateDto> FindAvailableDate(string? firstName, string? lastName, string specialization, DateTime date);
	}
}
