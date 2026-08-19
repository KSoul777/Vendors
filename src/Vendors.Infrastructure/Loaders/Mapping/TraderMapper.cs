using SqlServerLoader;
using Vendors.Domain.Vendors;

namespace Vendors.Infrastructure.Loaders.Mapping;

internal static class TraderMapper
{
    public static Vendor ToVendor(Trader trader) =>
        Vendor.Create(trader.Code, trader.Description, trader.Street).Value;

    public static Trader ToTrader(Vendor vendor) => new()
    {
        Code = vendor.Id,
        Description = vendor.Name,
        Street = vendor.Address
    };
}
