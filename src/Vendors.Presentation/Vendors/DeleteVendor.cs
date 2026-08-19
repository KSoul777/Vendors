using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Vendors.Application.Vendors;
using Vendors.Domain.Abstractions;
using Vendors.Presentation.Endpoints;

namespace Vendors.Presentation.Vendors;

internal sealed class DeleteVendor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("vendors/{id}", async (
            string id,
            IVendorService vendorService,
            CancellationToken cancellationToken) =>
        {
            Result result = await vendorService.DeleteAsync(id, cancellationToken);

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Vendors);
    }
}
