using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Vendors.Application.Abstractions;
using Vendors.Infrastructure.Features;
using Xunit;

namespace Vendors.UnitTests.Features;

public sealed class FeatureManagerTests
{
    private static FeatureManager Manager(params (string Key, string Value)[] settings)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings.ToDictionary(s => s.Key, s => (string?)s.Value))
            .Build();

        FeatureFlags flags = configuration.GetSection(FeatureFlags.SectionName).Get<FeatureFlags>() ?? new FeatureFlags();

        return new FeatureManager(Options.Create(flags));
    }

    [Theory]
    [InlineData("true")]
    [InlineData("True")]
    [InlineData("TRUE")]
    public void IsEnabled_Is_True_When_Flag_Is_Truthy(string value)
    {
        FeatureManager manager = Manager(($"{FeatureFlags.SectionName}:{nameof(FeatureFlags.UseSqlServerVendorLoader)}", value));

        Assert.True(manager.IsEnabled(FeatureFlag.UseSqlServerVendorLoader));
    }

    [Theory]
    [InlineData("false")]
    [InlineData("False")]
    [InlineData("FALSE")]
    public void IsEnabled_Is_False_When_Flag_Is_Not_Truthy(string value)
    {
        FeatureManager manager = Manager(($"{FeatureFlags.SectionName}:{nameof(FeatureFlags.UseSqlServerVendorLoader)}", value));

        Assert.False(manager.IsEnabled(FeatureFlag.UseSqlServerVendorLoader));
    }

    [Fact]
    public void IsEnabled_Is_False_When_Flag_Is_Missing_Or_Unknown()
    {
        FeatureManager manager = Manager();

        Assert.False(manager.IsEnabled(FeatureFlag.UseSqlServerVendorLoader));
        Assert.False(manager.IsEnabled("UnknownFeature"));
    }
}
