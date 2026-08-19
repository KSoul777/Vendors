namespace Vendors.Infrastructure.Configuration;

public sealed class VendorLoaderOptions
{
    public const string SectionName = "VendorLoaders";

    public FileLoaderOptions File { get; set; } = new();

    public SqlServerLoaderOptions SqlServer { get; set; } = new();

    public sealed class FileLoaderOptions
    {
        public string FilePath { get; set; } = "suppliers.txt";
    }

    public sealed class SqlServerLoaderOptions
    {
        public string Server { get; set; } = "server";

        public string UserId { get; set; } = "userid";

        public string Password { get; set; } = "password";
    }
}
