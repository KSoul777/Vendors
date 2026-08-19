using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Vendors.Application.Vendors;
using Vendors.Domain.Abstractions;
using Vendors.Presentation.Endpoints;

namespace Vendors.Presentation.Vendors;

internal sealed class CreateVendor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("vendors", async (
            CreateVendorRequest request,
            IVendorService vendorService,
            CancellationToken cancellationToken) =>
        {
            Result<VendorDto> result = await vendorService.CreateAsync(request, cancellationToken);

            return result.Match(
                vendor => Results.Created($"/vendors/{vendor.Id}", vendor),
                ApiResults.Problem);
        })
        .WithTags(Tags.Vendors);
    }
}
