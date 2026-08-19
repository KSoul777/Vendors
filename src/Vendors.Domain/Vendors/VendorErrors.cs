using Vendors.Domain.Abstractions;

namespace Vendors.Domain.Vendors;

public static class VendorErrors
{
    public static readonly Error IdRequired =
        Error.Validation("Vendors.IdRequired", "Vendor id is required.");

    public static readonly Error NameRequired =
        Error.Validation("Vendors.NameRequired", "Vendor name is required.");

    public static Error NotFound(string id) =>
        Error.NotFound("Vendors.NotFound", $"Vendor with id '{id}' was not found.");

    public static Error AlreadyExists(string id) =>
        Error.Conflict("Vendors.AlreadyExists", $"Vendor with id '{id}' already exists.");

    public static Error LoaderFailure(string description) =>
        Error.Failure("Vendors.LoaderFailure", description);
}
