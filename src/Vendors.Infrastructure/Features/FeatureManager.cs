using Microsoft.Extensions.Options;
using Vendors.Application.Abstractions;

namespace Vendors.Infrastructure.Features;

internal sealed class FeatureManager(IOptions<FeatureFlags> options) : IFeatureManager
{
    private readonly FeatureFlags _flags = options.Value;

    public bool IsEnabled(string feature) => feature switch
    {
        FeatureFlag.UseSqlServerVendorLoader => _flags.UseSqlServerVendorLoader,
        _ => false
    };
}
