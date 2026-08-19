using Vendors.Domain.Abstractions;

namespace Vendors.Domain.Vendors;

public sealed class Vendor
{
    private Vendor(string id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
    }

    public string Id { get; private set; }

    public string Name { get; private set; } 

    public string Address { get; private set; } 

    public static Result<Vendor> Create(string? id, string? name, string? address)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return Result.Failure<Vendor>(VendorErrors.IdRequired);
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Vendor>(VendorErrors.NameRequired);
        }

        return new Vendor(id.Trim(), name.Trim(), address?.Trim() ?? string.Empty);
    }

    public Result Update(string? name, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(VendorErrors.NameRequired);
        }

        Name = name.Trim();
        Address = address?.Trim() ?? string.Empty;
        return Result.Success();
    }
}
