namespace Vendors.Application.Abstractions;

public interface IFeatureManager
{
    bool IsEnabled(string feature);
}
