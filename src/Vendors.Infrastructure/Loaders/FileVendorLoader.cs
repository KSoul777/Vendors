using FileLoader;
using Microsoft.Extensions.Options;
using Vendors.Domain.Abstractions;
using Vendors.Domain.Vendors;
using Vendors.Infrastructure.Configuration;
using Vendors.Infrastructure.Loaders.Mapping;

namespace Vendors.Infrastructure.Loaders;

internal sealed class FileVendorLoader(IOptions<VendorLoaderOptions> options) : IVendorRepository
{
    private readonly Loader _loader = new(options.Value.File.FilePath);

    public Task<Result<IReadOnlyList<Vendor>>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Result.Of(() =>
        {
            IReadOnlyList<Vendor> vendors = _loader.LoadSuppliers().Select(SupplierMapper.ToVendor).ToList();
            return Result.Success(vendors);
        }, MapError));

    public Task<Result<Vendor>> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Result.Of(() => Result.Success(SupplierMapper.ToVendor(_loader.LoadSupplier(id))), MapError));

    public Task<Result<Vendor>> CreateAsync(Vendor vendor, CancellationToken cancellationToken = default) =>
        Task.FromResult(Result.Of(() =>
        {
            _loader.InsertSupplier(SupplierMapper.ToSupplier(vendor));
            return Result.Success(vendor);
        }, MapError));

    public Task<Result<Vendor>> UpdateAsync(Vendor vendor, CancellationToken cancellationToken = default) =>
        Task.FromResult(Result.Of(() =>
        {
            _loader.UpdateSupplier(SupplierMapper.ToSupplier(vendor));
            return Result.Success(vendor);
        }, MapError));

    public Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Result.Of(() =>
        {
            _loader.DeleteSupplier(id);
            return Result.Success();
        }, MapError));

    private static Error MapError(Exception exception) => exception switch
    {
        ApiException { StatusCode: 1 } ex => Error.NotFound("Vendors.NotFound", ex.Message),
        ApiException { StatusCode: 2 } ex => Error.Validation("Vendors.Validation", ex.Message),
        ApiException { StatusCode: 3 } ex => Error.Conflict("Vendors.Conflict", ex.Message),
        _ => Error.Failure("Vendors.LoaderFailure", exception.Message)
    };
}
