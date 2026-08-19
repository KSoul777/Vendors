using Microsoft.Extensions.Options;
using Vendors.Domain.Abstractions;
using Vendors.Domain.Vendors;
using Vendors.Infrastructure.Configuration;
using Vendors.Infrastructure.Loaders;
using Xunit;

namespace Vendors.UnitTests.Loaders;

public sealed class FileVendorLoaderTests
{
    private static FileVendorLoader CreateLoader(string filePath = "suppliers.txt")
    {
        var options = new VendorLoaderOptions();
        options.File.FilePath = filePath;
        return new FileVendorLoader(Options.Create(options));
    }

    [Fact]
    public async Task GetAllAsync_Maps_Suppliers_To_Unified_Vendors()
    {
        FileVendorLoader loader = CreateLoader();

        Result<IReadOnlyList<Vendor>> result = await loader.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count);
        Assert.Equal("1", result.Value[0].Id);
        Assert.Equal("Supp1", result.Value[0].Name);
        Assert.Equal("Add1", result.Value[0].Address);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_NotFound_For_Unknown_Id()
    {
        FileVendorLoader loader = CreateLoader();

        Result<Vendor> result = await loader.GetByIdAsync("does-not-exist");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task GetAllAsync_Fails_When_FilePath_Is_Wrong()
    {
        FileVendorLoader loader = CreateLoader("wrong-file.txt");

        Result<IReadOnlyList<Vendor>> result = await loader.GetAllAsync();

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Failure, result.Error.Type);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Vendor_For_Known_Id()
    {
        FileVendorLoader loader = CreateLoader();

        Result<Vendor> result = await loader.GetByIdAsync("1");

        Assert.True(result.IsSuccess);
        Assert.Equal("Supp1", result.Value.Name);
    }

    [Fact]
    public async Task CreateAsync_Returns_Conflict_For_Existing_Id()
    {
        FileVendorLoader loader = CreateLoader();
        Vendor vendor = Vendor.Create("1", "Duplicate", "Addr").Value; // id "1" is seeded

        Result<Vendor> result = await loader.CreateAsync(vendor);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }
}
