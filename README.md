# Vendor Web Service

ASP.NET Core (.NET 10) web service exposing vendor CRUD over one of two interchangeable
data loaders (`FileLoader` / `SqlServerLoader`), selected by configuration.

## Prerequisites

- .NET 10 SDK

## Run

```bash
dotnet run --project src/Vendors.Api
```

Open **Swagger UI** at `http://localhost:<port>/swagger` (the port is printed in the console).
No database or other dependency — the loaders hold data in memory.

## Choose the loader

Set the feature flag in [`src/Vendors.Api/appsettings.json`](src/Vendors.Api/appsettings.json):

```json
"Features": {
  "UseSqlServerVendorLoader": true
}
```

`true` → SqlServer loader, `false` (or absent) → File loader. Override without editing the file:

```bash
# PowerShell
$env:Features__UseSqlServerVendorLoader = "false"; dotnet run --project src/Vendors.Api
```

## Test

```bash
dotnet test Vendors.slnx
```
