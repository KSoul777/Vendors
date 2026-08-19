using Microsoft.Extensions.Options;
using SqlServerLoader;
using Vendors.Domain.Abstractions;
using Vendors.Domain.Vendors;
using Vendors.Infrastructure.Configuration;
using Vendors.Infrastructure.Loaders.Mapping;

namespace Vendors.Infrastructure.Loaders;

internal sealed class SqlServerVendorLoader(IOptions<VendorLoaderOptions> options) : IVendorRepository
{
    private readonly DataLoader _loader = new(
        options.Value.SqlServer.Server,
        options.Value.SqlServer.UserId,
        options.Value.SqlServer.Password);

    public Task<Result<IReadOnlyList<Vendor>>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Result.Of(async () =>
        {
            List<Trader> traders = await _loader.LoadTraders();
            IReadOnlyList<Vendor> vendors = traders.Select(TraderMapper.ToVendor).ToList();
            return Result.Success(vendors);
        }, MapError);

    public Task<Result<Vendor>> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        Result.Of(async () => Result.Success(TraderMapper.ToVendor(await _loader.LoadTrader(id))), MapError);

    public Task<Result<Vendor>> CreateAsync(Vendor vendor, CancellationToken cancellationToken = default) =>
        Result.Of(async () =>
        {
            await _loader.InsertTrader(TraderMapper.ToTrader(vendor));
            return Result.Success(vendor);
        }, MapError);

    public Task<Result<Vendor>> UpdateAsync(Vendor vendor, CancellationToken cancellationToken = default) =>
        Result.Of(async () =>
        {
            await _loader.UpdateTrader(TraderMapper.ToTrader(vendor));
            return Result.Success(vendor);
        }, MapError);

    public Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default) =>
        Result.Of(async () =>
        {
            await _loader.DeleteTrader(id);
            return Result.Success();
        }, MapError);

    private static Error MapError(Exception exception)
    {
        string message = exception.Message;

        if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return Error.NotFound("Vendors.NotFound", message);
        }

        if (message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
            return Error.Conflict("Vendors.Conflict", message);
        }

        if (message.Contains("required", StringComparison.OrdinalIgnoreCase))
        {
            return Error.Validation("Vendors.Validation", message);
        }

        return Error.Failure("Vendors.LoaderFailure", message);
    }
}
