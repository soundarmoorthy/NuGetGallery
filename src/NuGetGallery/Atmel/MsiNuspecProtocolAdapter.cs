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

using MsiDatabase = Microsoft.Deployment.WindowsInstaller.Database;
using System.Web;

namespace NuGetGallery
{
    public class MsiNuspecProtocolAdapter : INuspecProtocolAdapter
    {

        public MsiNuspecProtocolAdapter()
        {

        }

        public PackageRegistrationInfo ConstructRegistrationInfo(string fn)
        {
            MsiDatabase msi = new MsiDatabase(fn);
            return new PackageRegistrationInfo(Id(msi), Version(msi));
        }

	string Version(MsiDatabase db) 
	    => (string)db.ExecuteScalar("SELECT `Value` FROM " +
                           "`Property` WHERE `Property` = 'ProductVersion'");

	string Id(MsiDatabase db) 
	    => (string)db.ExecuteScalar("SELECT `Value` FROM " +
                     " `Property` WHERE `Property` = 'UpgradeCode'");

        public PackageMetadata ConstructMetadata(FileStreamContext context) 
	    => ConstructWith(NuspecDictionary(context));

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

        private Dictionary<string, string> NuspecDictionary(FileStreamContext context)
        {
            MsiDatabase msi = new MsiDatabase(context.Name);

            var dict = new Dictionary<string, string>();
            dict.Add(PackageMetadata.IdTag, Id(msi));
                
            dict.Add(PackageMetadata.VersionTag, Version(msi));
            //SOUNDAR : Fix the icon path. It expects a http URI, whereas this is the relative path to vsix
            dict.Add(PackageMetadata.IconUrlTag, "");
            dict.Add(PackageMetadata.projectUrlTag, "");
            //SOUNDAR : Need to find license URL or serve it.
            dict.Add(PackageMetadata.licenseUrlTag, "");
            dict.Add(PackageMetadata.copyrightTag, "");
            dict.Add(PackageMetadata.descriptionTag, "");
            dict.Add(PackageMetadata.releaseNotesTag, "");
            dict.Add(PackageMetadata.requireLicenseAcceptanceTag, "true");
            dict.Add(PackageMetadata.summaryTag, msi.SummaryInfo.Comments);
            dict.Add(PackageMetadata.titleTag, msi.SummaryInfo.Title);
            dict.Add(PackageMetadata.tagsTag, "msi");
            dict.Add(PackageMetadata.languagesTag, "en-US");
            dict.Add(PackageMetadata.ownersTag, context.Username);
            dict.Add(PackageMetadata.commaseparatedAuthorsTag, msi.SummaryInfo.Author);
            return dict;
        }
    }
}
