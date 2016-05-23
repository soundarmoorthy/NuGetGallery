using NuGetGallery;
using NuGetGallery.Packaging;
using System.IO;
using System.Web;

namespace NuGetGallery
{
    public interface INuspecProtocolAdapter
    {
        PackageMetadata Metadata(FileStream context);
        PackageRegistrationInfo ConstructRegistrationInfo(string fileName);
    }
}