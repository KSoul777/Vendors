using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Vendors.Application.Vendors;
using Vendors.Domain.Abstractions;
using Vendors.Presentation.Endpoints;

namespace Vendors.Presentation.Vendors;

internal sealed class UpdateVendor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("vendors/{id}", async (
            string id,
            UpdateVendorRequest request,
            IVendorService vendorService,
            CancellationToken cancellationToken) =>
        {
            Result<VendorDto> result = await vendorService.UpdateAsync(id, request, cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Vendors);
    }
}
