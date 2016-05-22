using System.IO;

namespace NuGetGallery
{
    public class PackageRegistrationInfo
    {
        public string Id { get; }
        public string Version { get; }

	public PackageRegistrationInfo(string id, string version)
        {
            this.Id = id;
            this.Version = version;
        }
    }
}