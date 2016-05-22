using NuGetGallery;
using NuGetGallery.Packaging;
using System.IO;
using System.Web;

namespace NuGetGallery
{
    public interface INuspecProtocolAdapter
    {
        PackageMetadata ConstructMetadata(FileStreamContext context);
        PackageRegistrationInfo ConstructRegistrationInfo(Stream stream, string fileName);
    }
}