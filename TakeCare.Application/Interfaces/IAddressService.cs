using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
	public interface IAddressService
	{
		Task<string> CreateAddressAsync(Address address);

		Task<Address> ReadAddressAsync(int addressId);

		Task<string> UpdateAddressAsync(Address address);

		Task<string> DeleteAddressAsync(int addressId);
	}
}
