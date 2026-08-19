using Vendors.Domain.Abstractions;
using Vendors.Domain.Vendors;
using Xunit;

namespace Vendors.UnitTests.Domain;

public sealed class VendorTests
{
    [Fact]
    public void Create_Trims_And_Succeeds_With_Valid_Input()
    {
        Result<Vendor> result = Vendor.Create(" 1 ", " Acme ", " Main Road ");

        Assert.True(result.IsSuccess);
        Assert.Equal("1", result.Value.Id);
        Assert.Equal("Acme", result.Value.Name);
        Assert.Equal("Main Road", result.Value.Address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Fails_When_Id_Missing(string? id)
    {
        Result<Vendor> result = Vendor.Create(id, "Acme", "Main Road");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public void Create_Fails_When_Name_Missing()
    {
        Result<Vendor> result = Vendor.Create("1", "", "Main Road");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public void Create_Allows_Empty_Address()
    {
        Result<Vendor> result = Vendor.Create("1", "Acme", null);

        Assert.True(result.IsSuccess);
        Assert.Equal(string.Empty, result.Value.Address);
    }
}
