using NSubstitute;
using Vendors.Application.Vendors;
using Vendors.Domain.Abstractions;
using Vendors.Domain.Vendors;
using Xunit;

namespace Vendors.UnitTests.Application;

public sealed class VendorServiceTests
{
    private readonly IVendorRepository _repository = Substitute.For<IVendorRepository>();
    private readonly VendorService _sut;

    public VendorServiceTests() => _sut = new VendorService(_repository);

    [Fact]
    public async Task GetAllAsync_Maps_Vendors_To_Dtos()
    {
        Vendor vendor = Vendor.Create("1", "Acme", "Road").Value;
        _repository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Result.Success<IReadOnlyList<Vendor>>([vendor]));

        Result<IReadOnlyList<VendorDto>> result = await _sut.GetAllAsync();

        Assert.True(result.IsSuccess);
        VendorDto dto = Assert.Single(result.Value);
        Assert.Equal("1", dto.Id);
        Assert.Equal("Acme", dto.Name);
    }

    [Fact]
    public async Task GetAllAsync_Propagates_Repository_Failure()
    {
        Error error = Error.Failure("Vendors.LoaderFailure", "boom");
        _repository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Result.Failure<IReadOnlyList<Vendor>>(error));

        Result<IReadOnlyList<VendorDto>> result = await _sut.GetAllAsync();

        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_Propagates_NotFound()
    {
        _repository.GetByIdAsync("nope", Arg.Any<CancellationToken>())
            .Returns(Result.Failure<Vendor>(VendorErrors.NotFound("nope")));

        Result<VendorDto> result = await _sut.GetByIdAsync("nope");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task CreateAsync_Fails_Validation_And_Skips_Repository_For_Invalid_Input()
    {
        var request = new CreateVendorRequest(Id: "", Name: "Acme", Address: "Road");

        Result<VendorDto> result = await _sut.CreateAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        await _repository.DidNotReceive().CreateAsync(Arg.Any<Vendor>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_Persists_And_Returns_Dto_For_Valid_Input()
    {
        var request = new CreateVendorRequest(Id: "1", Name: "Acme", Address: "Road");
        _repository.CreateAsync(Arg.Any<Vendor>(), Arg.Any<CancellationToken>())
            .Returns(call => Result.Success(call.Arg<Vendor>()));

        Result<VendorDto> result = await _sut.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal("1", result.Value.Id);
        await _repository.Received(1).CreateAsync(Arg.Any<Vendor>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_Delegates_To_Repository()
    {
        _repository.DeleteAsync("1", Arg.Any<CancellationToken>()).Returns(Result.Success());

        Result result = await _sut.DeleteAsync("1");

        Assert.True(result.IsSuccess);
        await _repository.Received(1).DeleteAsync("1", Arg.Any<CancellationToken>());
    }
}
