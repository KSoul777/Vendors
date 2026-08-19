using Vendors.Domain.Abstractions;
using Vendors.Domain.Vendors;

namespace Vendors.Application.Vendors;

public sealed class VendorService(IVendorRepository repository) : IVendorService
{
    public async Task<Result<IReadOnlyList<VendorDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        Result<IReadOnlyList<Vendor>> result = await repository.GetAllAsync(cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<IReadOnlyList<VendorDto>>(result.Error);
        }

        IReadOnlyList<VendorDto> responses = result.Value.Select(ToResponse).ToList();
        return Result.Success(responses);
    }

    public async Task<Result<VendorDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        Result<Vendor> result = await repository.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess
            ? ToResponse(result.Value)
            : Result.Failure<VendorDto>(result.Error);
    }

    public async Task<Result<VendorDto>> CreateAsync(CreateVendorRequest request, CancellationToken cancellationToken = default)
    {
        Result<Vendor> vendor = Vendor.Create(request.Id, request.Name, request.Address);
        if (vendor.IsFailure)
        {
            return Result.Failure<VendorDto>(vendor.Error);
        }

        Result<Vendor> result = await repository.CreateAsync(vendor.Value, cancellationToken);

        return result.IsSuccess
            ? ToResponse(result.Value)
            : Result.Failure<VendorDto>(result.Error);
    }

    public async Task<Result<VendorDto>> UpdateAsync(string id, UpdateVendorRequest request, CancellationToken cancellationToken = default)
    {
        Result<Vendor> vendor = Vendor.Create(id, request.Name, request.Address);
        if (vendor.IsFailure)
        {
            return Result.Failure<VendorDto>(vendor.Error);
        }

        Result<Vendor> result = await repository.UpdateAsync(vendor.Value, cancellationToken);

        return result.IsSuccess
            ? ToResponse(result.Value)
            : Result.Failure<VendorDto>(result.Error);
    }

    public Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(id, cancellationToken);

    private static VendorDto ToResponse(Vendor vendor) => new(vendor.Id, vendor.Name, vendor.Address);
}
