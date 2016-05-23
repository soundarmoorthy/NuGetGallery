using System;
using System.IO;
using NuGetGallery.Packaging;
using System.Collections.Generic;
using NuGet.Versioning;
using VSIXParser;
using System.Linq;
using NuGet.Packaging;
using NuGet.Frameworks;
using System.Diagnostics;
using System.Web;

namespace NuGetGallery
{
    public class VsixNuspecProtocolAdapter : INuspecProtocolAdapter
    {

        public VsixNuspecProtocolAdapter()
        {
        }

        public PackageRegistrationInfo ConstructRegistrationInfo(string fileName)
        {
            var vsix = VsixItem(fileName);
            return new PackageRegistrationInfo(vsix.VsixId, vsix.VsixVersion);
        }

        private VsixItem VsixItem(string fileName)
        {
            var content = GetContentFromStream(fileName);
            var vsixItem = VsixRepository.Read(content, fileName);
            return vsixItem;
        }

        private byte[] GetContentFromStream(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                Int32 count = (Int32)stream.Length;
                byte[] content = new byte[count];
                stream.Read(content, 0, count);
                return content;
            }
        }

        public PackageMetadata Metadata(FileStream context)
            => ConstructWith(Metadata(VsixItem(context.Name)));


        private PackageMetadata ConstructWith(Dictionary<string, string> dict) 
	    =>  new PackageMetadata(dict, DepGroups(), FxGroups(), new NuGetVersion("7.0"));

        IEnumerable<PackageDependencyGroup> DepGroups() 
	    =>  new[] {
                        new PackageDependencyGroup(NuGetFramework.AnyFramework,
                        Enumerable.Empty<NuGet.Packaging.Core.PackageDependency>())
                      };


        IEnumerable<FrameworkSpecificGroup> FxGroups() 
	    =>  new[] {
                        new FrameworkSpecificGroup(NuGetFramework.AnyFramework, Enumerable.Empty<string>())
                      };

        private Dictionary<string, string> Metadata(VSIXParser.VsixItem vsixItem)
        {

            var dict = new Dictionary<string, string>();

            dict.Add(PackageMetadata.IdTag, vsixItem.VsixId);
            dict.Add(PackageMetadata.VersionTag, vsixItem.VsixVersion);
            //SOUNDAR : Fix the icon path. It expects a http URI, whereas this is the relative path to vsix
            dict.Add(PackageMetadata.IconUrlTag, vsixItem.Icon);
            dict.Add(PackageMetadata.projectUrlTag, vsixItem.MoreInfo);
            //SOUNDAR : Need to find license URL or serve it.
            dict.Add(PackageMetadata.licenseUrlTag, "http://www.apache.org/licenses/LICENSE-2.0");
            dict.Add(PackageMetadata.copyrightTag, "");
            dict.Add(PackageMetadata.descriptionTag, vsixItem.Description);
            dict.Add(PackageMetadata.releaseNotesTag, "https://en.wikipedia.org/wiki/Release_notes");
            dict.Add(PackageMetadata.requireLicenseAcceptanceTag, "true");
            dict.Add(PackageMetadata.summaryTag, vsixItem.Description);
            dict.Add(PackageMetadata.titleTag, vsixItem.DisplayName);
            dict.Add(PackageMetadata.tagsTag, vsixItem.Type);
            dict.Add(PackageMetadata.languagesTag, "en-US");
            dict.Add(PackageMetadata.ownersTag,"" );
            dict.Add(PackageMetadata.commaseparatedAuthorsTag, vsixItem.Publisher);
            return dict;
        }
    }
}
