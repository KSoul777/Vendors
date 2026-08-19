using Vendors.Api.Middleware;
using Vendors.Infrastructure;
using Vendors.Presentation.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddVendorsModule(builder.Configuration);

builder.Services.AddEndpoints(Vendors.Presentation.AssemblyReference.Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapEndpoints();

await app.RunAsync();

public partial class Program;
