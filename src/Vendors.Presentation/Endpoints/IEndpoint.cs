using Microsoft.AspNetCore.Routing;

namespace Vendors.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
