using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendors.Application.Abstractions;
using Vendors.Domain.Vendors;
using Vendors.Infrastructure;
using Vendors.Infrastructure.Loaders;
using Xunit;

namespace Vendors.UnitTests.Infrastructure;

public sealed class InfrastructureModuleTests
{
    [Theory]
    [InlineData("true", typeof(SqlServerVendorLoader))]
    [InlineData("false", typeof(FileVendorLoader))]
    public void AddVendorsModule_Resolves_The_Loader_Selected_By_The_Feature_Flag(string useSqlServer, Type expectedLoader)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddVendorsModule(Configuration(($"{FeatureFlags.SectionName}:UseSqlServerVendorLoader", useSqlServer)));

        using ServiceProvider provider = services.BuildServiceProvider();
        IVendorRepository repository = provider.GetRequiredService<IVendorRepository>();

        Assert.IsType(expectedLoader, repository);
    }

    private static IConfiguration Configuration(params (string Key, string Value)[] settings) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(settings.ToDictionary(s => s.Key, s => (string?)s.Value))
            .Build();
}
