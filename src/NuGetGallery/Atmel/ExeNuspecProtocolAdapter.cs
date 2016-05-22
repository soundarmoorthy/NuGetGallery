using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Packaging;
using System.Diagnostics;
using System.IO;
using NuGet.Packaging;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace NuGetGallery
{
    public class ExeNuspecProtocolAdapter : INuspecProtocolAdapter
    {
        public PackageMetadata ConstructMetadata(FileStreamContext context) =>
ConstructWith(NuspecDictionary(context));
	   

        public PackageRegistrationInfo ConstructRegistrationInfo(Stream stream, string fn) =>
            new PackageRegistrationInfo(Id(fn), VersionInfo(fn));

        string Id(string fn) => new FileInfo(fn).CreationTimeUtc.ToFileTimeUtc().ToString();

        string VersionInfo(string fn) => FileVersionInfo.GetVersionInfo(fn).ProductVersion;

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


        private string GetGoodFileName(FileVersionInfo info)
        {
            var name = info.ProductName ?? info.InternalName ?? info.FileName;
            name = name.Replace('_', ' ').Replace('-', ' ').Replace('.', ' ');
            return string.Concat(name, " extension for Atmel Studido");
        }

        private Dictionary<string, string> NuspecDictionary(FileStreamContext context)
        {

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(context.Name);
            var dict = new Dictionary<string, string>();
            dict.Add(PackageMetadata.IdTag,
                string.Join("_", info.ProductName, Guid.NewGuid().ToString()));

            dict.Add(PackageMetadata.VersionTag, info.ProductVersion ?? info.FileVersion ?? "");
            //SOUNDAR : Fix the icon path. It expects a http URI, whereas this is the relative path to vsix
            dict.Add(PackageMetadata.IconUrlTag, "");
            dict.Add(PackageMetadata.projectUrlTag, "");
            //SOUNDAR : Need to find license URL or serve it.
            dict.Add(PackageMetadata.licenseUrlTag, "");
            dict.Add(PackageMetadata.copyrightTag, info.LegalCopyright ?? "");
            dict.Add(PackageMetadata.descriptionTag, info.FileDescription ?? info.Comments ?? "");
            dict.Add(PackageMetadata.releaseNotesTag, "");
            dict.Add(PackageMetadata.requireLicenseAcceptanceTag, "true");
            dict.Add(PackageMetadata.summaryTag, info.FileDescription ?? info.Comments ?? "");
            dict.Add(PackageMetadata.titleTag, GetGoodFileName(info));
            dict.Add(PackageMetadata.tagsTag, "msi");
            dict.Add(PackageMetadata.languagesTag, info.Language ?? "en-US");
            dict.Add(PackageMetadata.ownersTag, info.CompanyName ?? "");
            dict.Add(PackageMetadata.commaseparatedAuthorsTag, context.Username);
            return dict;
        }
    }
}