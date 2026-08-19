namespace Vendors.Application.Abstractions;

public sealed class FeatureFlags
{
    public const string SectionName = "Features";

    public bool UseSqlServerVendorLoader { get; set; }
}
