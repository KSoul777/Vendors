namespace Vendors.Application.Vendors;

public sealed record CreateVendorRequest(string Id, string Name, string? Address);

public sealed record UpdateVendorRequest(string Name, string? Address);

public sealed record VendorDto(string Id, string Name, string Address);
