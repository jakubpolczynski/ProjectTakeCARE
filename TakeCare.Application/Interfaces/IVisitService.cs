using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
	public interface IVisitService
	{
		Task<IEnumerable<DateTime>> FindAvailableDate(string firstName, string lastName, DateTime date);
	}
}
