using Vendors.Domain.Abstractions;

namespace Vendors.Domain.Vendors;

/// <summary>
/// Persistence contract for the <see cref="Vendor"/> aggregate. Implementations fold their
/// data source's failures into a <see cref="Result"/> — no exceptions escape.
/// </summary>
public interface IVendorRepository
{
    Task<Result<IReadOnlyList<Vendor>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<Vendor>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<Result<Vendor>> CreateAsync(Vendor vendor, CancellationToken cancellationToken = default);

    Task<Result<Vendor>> UpdateAsync(Vendor vendor, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
