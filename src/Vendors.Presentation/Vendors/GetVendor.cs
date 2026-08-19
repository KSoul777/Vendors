using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Vendors.Application.Vendors;
using Vendors.Domain.Abstractions;
using Vendors.Presentation.Endpoints;

namespace Vendors.Presentation.Vendors;

internal sealed class GetVendor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("vendors/{id}", async (
            string id,
            IVendorService vendorService,
            CancellationToken cancellationToken) =>
        {
            Result<VendorDto> result = await vendorService.GetByIdAsync(id, cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Vendors);
    }
}
