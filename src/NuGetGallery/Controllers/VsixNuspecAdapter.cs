using System;
using System.IO;
using NuGetGallery.Packaging;
using System.Collections.Generic;
using NuGet.Versioning;
using VSIXParser;
using System.Linq;
using NuGet.Packaging;
using NuGet.Frameworks;

namespace NuGetGallery
{
    internal class VsixNuspecAdapter
    {
        private readonly FileStreamInfo FileStreamInfo;

        internal VsixNuspecAdapter(FileStreamInfo fileStreamInfo)
        {
            this.FileStreamInfo = fileStreamInfo;
        }
	

	internal static PackageMetadata Construct(FileStreamInfo info)
        {
            var adapter = new VsixNuspecAdapter(info);
            return adapter.ToNuspecPackageMetadata();
        }

        internal PackageMetadata ToNuspecPackageMetadata()
        {

            var content = File.ReadAllBytes(FileStreamInfo.Name);
            var vsixItem = VsixRepository.Read(content, FileStreamInfo.Name);

            var metadata = ConstructNuspecComplaintDictionary(vsixItem);
            var packageDependencyGroups = Enumerable.Empty<PackageDependencyGroup>();
            var group = new PackageDependencyGroup(NuGetFramework.AnyFramework, Enumerable.Empty<NuGet.Packaging.Core.PackageDependency>());
            var fxGroup = new FrameworkSpecificGroup(NuGetFramework.AnyFramework, Enumerable.Empty<string>());
            var nuspecPackageMetadata = new PackageMetadata(metadata, new[] { group }, new[] { fxGroup }, new NuGetVersion(0, 0, 0));
            return nuspecPackageMetadata;
        }

        private Dictionary<string,string> ConstructNuspecComplaintDictionary(VSIXParser.VsixItem vsixItem)
        {
            //The Stream.Read method can process files of size 2 GB maximum. 
            //This puts a constraint on the maximum size of the extension to
            //not exceed 2 GB

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
            dict.Add(PackageMetadata.ownersTag, vsixItem.Publisher);
            dict.Add(PackageMetadata.commaseparatedAuthorsTag, "");
            return dict;
        }

    }
}