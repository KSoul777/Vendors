using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendors.Application.Abstractions;
using Vendors.Application.Vendors;
using Vendors.Domain.Vendors;
using Vendors.Infrastructure.Configuration;
using Vendors.Infrastructure.Features;
using Vendors.Infrastructure.Loaders;

namespace Vendors.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddVendorsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<VendorLoaderOptions>(configuration.GetSection(VendorLoaderOptions.SectionName));
        services.Configure<FeatureFlags>(configuration.GetSection(FeatureFlags.SectionName));

        services.AddSingleton<IFeatureManager, FeatureManager>();

        FeatureFlags featureFlags = configuration.GetSection(FeatureFlags.SectionName).Get<FeatureFlags>() ?? new FeatureFlags();

        if (featureFlags.UseSqlServerVendorLoader)
        {
            services.AddScoped<IVendorRepository, SqlServerVendorLoader>();
        }
        else
        {
            services.AddScoped<IVendorRepository, FileVendorLoader>();
        }

        services.AddScoped<IVendorService, VendorService>();

        return services;
    }
}
