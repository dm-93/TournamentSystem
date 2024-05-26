using TournamentSystemModels;

namespace TournamentSystemDataSource.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Address> CreateAddressAsync(Address address, CancellationToken cancellationToken);
        Task DeleteAddressAsync(int addressId, CancellationToken cancellationToken);
        Task<Address> GetAddressByIdAsync(int addressId, CancellationToken cancellationToken);
        Task<IEnumerable<Address>> GetAddressesAsync(CancellationToken cancellationToken);
        Task<Address> UpdateAddressAsync(Address updatedAddress, CancellationToken cancellationToken);
    }
}