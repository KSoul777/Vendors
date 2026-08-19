using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Vendors.Application.Vendors;
using Vendors.Domain.Abstractions;
using Vendors.Presentation.Endpoints;

namespace Vendors.Presentation.Vendors;

internal sealed class GetVendors : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("vendors", async (
            IVendorService vendorService,
            CancellationToken cancellationToken) =>
        {
            Result<IReadOnlyList<VendorDto>> result = await vendorService.GetAllAsync(cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Vendors);
    }
}
