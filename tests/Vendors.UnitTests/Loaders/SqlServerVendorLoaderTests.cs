using Microsoft.Extensions.Options;
using Vendors.Domain.Abstractions;
using Vendors.Domain.Vendors;
using Vendors.Infrastructure.Configuration;
using Vendors.Infrastructure.Loaders;
using Xunit;

namespace Vendors.UnitTests.Loaders;

public sealed class SqlServerVendorLoaderTests
{
    private static SqlServerVendorLoader CreateLoader(
        string server = "server",
        string userId = "userid",
        string password = "password")
    {
        var options = new VendorLoaderOptions();
        options.SqlServer.Server = server;
        options.SqlServer.UserId = userId;
        options.SqlServer.Password = password;
        return new SqlServerVendorLoader(Options.Create(options));
    }

    [Fact]
    public async Task GetAllAsync_Maps_Traders_To_Unified_Vendors()
    {
        SqlServerVendorLoader loader = CreateLoader();

        Result<IReadOnlyList<Vendor>> result = await loader.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal("sql1", result.Value[0].Id);         // Trader.Code        -> Vendor.Id
        Assert.Equal("sSupp1", result.Value[0].Name);     // Trader.Description -> Vendor.Name
        Assert.Equal("sqlAdd1", result.Value[0].Address); // Trader.Street      -> Vendor.Address
    }

    [Fact]
    public async Task GetByIdAsync_Returns_NotFound_For_Unknown_Code()
    {
        SqlServerVendorLoader loader = CreateLoader();

        Result<Vendor> result = await loader.GetByIdAsync("nope");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task GetAllAsync_Fails_On_Wrong_Connection_Info()
    {
        SqlServerVendorLoader loader = CreateLoader(server: "bad-server");

        Result<IReadOnlyList<Vendor>> result = await loader.GetAllAsync();

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Failure, result.Error.Type);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Vendor_For_Known_Code()
    {
        SqlServerVendorLoader loader = CreateLoader();

        Result<Vendor> result = await loader.GetByIdAsync("sql1");

        Assert.True(result.IsSuccess);
        Assert.Equal("sSupp1", result.Value.Name);
    }

    [Fact]
    public async Task CreateAsync_Returns_Conflict_For_Existing_Code()
    {
        SqlServerVendorLoader loader = CreateLoader();
        Vendor vendor = Vendor.Create("sql1", "Duplicate", "Addr").Value; // code "sql1" is seeded

        Result<Vendor> result = await loader.CreateAsync(vendor);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }
}
