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
using FileVer = System.Diagnostics.FileVersionInfo;
using Summary = System.Collections.Generic.Dictionary<string, string>;
using System.Globalization;

namespace NuGetGallery
{
    public class ExeNuspecProtocolAdapter : INuspecProtocolAdapter
    {
        public PackageMetadata Metadata(FileStream context) =>
            ConstructWith(Summary(context));
	   

        public PackageRegistrationInfo ConstructRegistrationInfo(string fn) =>
            new PackageRegistrationInfo(Id(fn), VersionInfo(fn));

        string Id(string fn) => GenerateComputableId(FileVer.GetVersionInfo(fn));

        string VersionInfo(string fn) => GetVersion(FileVer.GetVersionInfo(fn));

        private PackageMetadata ConstructWith(Summary dict) 
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


        private string GetGoodFileName(FileVer info)
        {
            var name = info.ProductName ?? info.InternalName ?? info.FileName;
            name = name.Replace('_', ' ').Replace('-', ' ').Replace('.', ' ');
            return string.Concat(name, " extension for Atmel Studido");
        }

        private Summary Summary(FileStream context)
        {
            FileVer info = FileVer.GetVersionInfo(context.Name);
            var dict = new Summary();
            string id = GenerateComputableId(info);

            dict.Add(PackageMetadata.IdTag, id);

            dict.Add(PackageMetadata.VersionTag, GetVersion(info));
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

            dict.Add(PackageMetadata.languagesTag, GetLanguage(info));
            dict.Add(PackageMetadata.ownersTag, "");
            dict.Add(PackageMetadata.commaseparatedAuthorsTag, info.CompanyName ?? "");
            return dict;
        }

	private static string GetLanguage(FileVer f)
        {
            var language = "en-US"; //By default assume English (United States)
            if (f.Language != null)
            {
                var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                var c = cultures.Where(x => String.Equals(x.EnglishName, f.Language, StringComparison.OrdinalIgnoreCase));
                if (c.Any())
                    language =  c.First().Name;
            }
            return language;
        }

        private static string GetVersion(FileVer info)
        {
            Version result = null;
            var success = Version.TryParse(info.ProductVersion, out result);

            if (!success)
                success = Version.TryParse(info.FileVersion, out result);

            //SOUNDAR : If we are not able to find the version , we should leave that to the user 
            //to edit from the UI. Currently it's not the case. We set it to "1.0.0" for now.
            if (!success)
                result = new Version("1.0.0");

            return result.ToString();
        }

        private static string GenerateComputableId(FileVer info)
        {
            var id = info.Comments?.Replace(' ', '-');

            if (string.IsNullOrEmpty(id))
                id = Path.GetFileNameWithoutExtension(info.OriginalFilename).Replace(' ', '-');

            if (string.IsNullOrEmpty(id))
                id = Path.GetFileNameWithoutExtension(info.InternalName).Replace(' ', '-');

            return id;
        }
    }
}