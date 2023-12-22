using TakeCare.Application.Interfaces;
using TakeCare.Database.Entity;

namespace TakeCare.Application.Services
{
	public class AddressService : IAddressService
	{
		public Task<string> CreateAddressAsync(Address address)
		{
			throw new NotImplementedException();
		}

		public Task<string> DeleteAddressAsync(int addressId)
		{
			throw new NotImplementedException();
		}

		public Task<Address> ReadAddressAsync(int addressId)
		{
			throw new NotImplementedException();
		}

		public Task<string> UpdateAddressAsync(Address address)
		{
			throw new NotImplementedException();
		}
	}
}
