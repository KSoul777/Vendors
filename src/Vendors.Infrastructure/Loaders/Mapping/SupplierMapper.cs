using FileLoader;
using Vendors.Domain.Vendors;

namespace Vendors.Infrastructure.Loaders.Mapping;

internal static class SupplierMapper
{
    public static Vendor ToVendor(Supplier supplier) =>
        Vendor.Create(supplier.Id, supplier.Name, supplier.Address).Value;

    public static Supplier ToSupplier(Vendor vendor) => new()
    {
        Id = vendor.Id,
        Name = vendor.Name,
        Address = vendor.Address
    };
}
