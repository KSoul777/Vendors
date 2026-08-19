using Vendors.Domain.Abstractions;

namespace Vendors.Application.Vendors;

public interface IVendorService
{
    Task<Result<IReadOnlyList<VendorDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<VendorDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<Result<VendorDto>> CreateAsync(CreateVendorRequest request, CancellationToken cancellationToken = default);

    Task<Result<VendorDto>> UpdateAsync(string id, UpdateVendorRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
