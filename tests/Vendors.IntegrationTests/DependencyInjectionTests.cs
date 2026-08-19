using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Vendors.Application.Vendors;
using Vendors.Domain.Vendors;
using Xunit;

namespace Vendors.IntegrationTests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void IVendorService_And_Its_Repository_Resolve_From_The_Booted_App()
    {
        using WebApplicationFactory<Program> factory = new();
        using IServiceScope scope = factory.Services.CreateScope();

        Assert.NotNull(scope.ServiceProvider.GetRequiredService<IVendorService>());
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<IVendorRepository>());
    }

    [Fact]
    public async Task GetVendors_Endpoint_Returns_Success()
    {
        using WebApplicationFactory<Program> factory = new();
        using HttpClient client = factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/vendors");

        response.EnsureSuccessStatusCode();
    }
}
