using NuGetGallery;
using NuGetGallery.Packaging;
using System.IO;
using System.Web;

namespace NuGetGallery
{
    public interface INuspecProtocolAdapter
    {
        PackageMetadata ConstructMetadata(FileStream context);
        PackageRegistrationInfo ConstructRegistrationInfo(string fileName);
    }
}