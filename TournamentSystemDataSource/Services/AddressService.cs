using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TournamentSystemDataSource.Services.Interfaces;
using TournamentSystemModels;

namespace TournamentSystemDataSource.Services
{
    internal class AddressService : IAddressService
    {
        private readonly GeneralContext _context;
        private readonly ILogger<AddressService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(GeneralContext context, ILogger<AddressService> logger, IUnitOfWork unitOfWork)
        {
            _context = context;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Address>> GetAddressesAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving addresses...");

            var addresses = await _context.Addresses.ToListAsync(cancellationToken);

            _logger.LogInformation("Addresses retrieved successfully.");

            return addresses;
        }

        public async Task<Address> GetAddressByIdAsync(int addressId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving address with ID: {addressId}...");

            var address = await _context.Addresses.FindAsync(addressId);

            if (address == null)
            {
                _logger.LogWarning($"Address with ID {addressId} not found.");
                throw new ArgumentException($"Адрес с Id {addressId} не найден.");
            }

            _logger.LogInformation($"Address with ID {addressId} retrieved successfully.");

            return address;
        }

        public async Task<Address> CreateAddressAsync(Address address, CancellationToken cancellationToken)
        {
            if (address == null)
            {
                throw new ArgumentNullException($"{nameof(address)} не может быть равно null.");
            }

            _logger.LogInformation("Creating a new address...");

            var res = await _context.Addresses.AddAsync(address);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation("Address created successfully.");

            return res.Entity;
        }

        public async Task<Address> UpdateAddressAsync(Address updatedAddress, CancellationToken cancellationToken)
        {
            if (updatedAddress == null)
            {
                throw new ArgumentNullException($"{nameof(updatedAddress)} не может быть равно null.");
            }

            _logger.LogInformation($"Updating address with ID: {updatedAddress.Id}...");

            var existingAddress = await _context.Addresses.FindAsync(updatedAddress.Id);

            if (existingAddress == null)
            {
                _logger.LogWarning($"Address with ID {updatedAddress.Id} not found.");
                throw new ArgumentException($"Адрес с Id {updatedAddress.Id} не найден.");
            }

            _context.Entry(existingAddress).CurrentValues.SetValues(updatedAddress);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"Address with ID {updatedAddress.Id} updated successfully.");

            return existingAddress;
        }

        public async Task DeleteAddressAsync(int addressId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting address with ID: {addressId}...");

            var address = await _context.Addresses.FindAsync(addressId);

            if (address == null)
            {
                _logger.LogWarning($"Address with ID {addressId} not found.");
                throw new ArgumentException($"Адрес с Id {addressId} не найден.");
            }

            _context.Addresses.Remove(address);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"Address with ID {addressId} deleted successfully.");
        }
    }
}