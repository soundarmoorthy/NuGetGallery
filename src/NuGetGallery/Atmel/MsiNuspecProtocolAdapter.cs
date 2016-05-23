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

using PM = NuGetGallery.Packaging.PackageMetadata;
using PkgInfo = NuGetGallery.PackageRegistrationInfo;
using MSIdb = Microsoft.Deployment.WindowsInstaller.Database;
using Summary = System.Collections.Generic.Dictionary<string, string>;

namespace NuGetGallery
{
    public class MsiNuspecProtocolAdapter : INuspecProtocolAdapter
    {

        public MsiNuspecProtocolAdapter()
        {

        }

        public PkgInfo ConstructRegistrationInfo(string fn)
        {
            using (var msi = new MSIdb(fn))
            {
                var info = new PkgInfo(Id(msi), Version(msi));
                msi.Close();
                return info;
            }
        }

	string Version(MSIdb db) 
	    => (string)db.ExecuteScalar("SELECT `Value` FROM " +
                           "`Property` WHERE `Property` = 'ProductVersion'");

	string Id(MSIdb db) 
	    => (string)db.ExecuteScalar("SELECT `Value` FROM " +
                     " `Property` WHERE `Property` = 'UpgradeCode'");

        public PM Metadata(FileStream context) 
	    => Construct(WithSummary(context));

        private PM Construct(Summary summ) 
	    =>  new PM(summ, DepGroups(), FxGroups(), new NuGetVersion("7.0"));

        IEnumerable<PackageDependencyGroup> DepGroups() 
	    =>  new[] {
                        new PackageDependencyGroup(NuGetFramework.AnyFramework,
                        Enumerable.Empty<NuGet.Packaging.Core.PackageDependency>())
                      };


        IEnumerable<FrameworkSpecificGroup> FxGroups() 
	    =>  new[] {
                        new FrameworkSpecificGroup(NuGetFramework.AnyFramework, Enumerable.Empty<string>())
                      };

        private Summary WithSummary(FileStream context)
        {
            using (MSIdb msi = new MSIdb(context.Name))
            {
                var dict = new Summary();
                dict.Add(PM.IdTag, Id(msi));

                dict.Add(PM.VersionTag, Version(msi));
                //SOUNDAR : Fix the icon path. It expects a http URI, whereas this is the relative path to vsix
                dict.Add(PM.IconUrlTag, "");
                dict.Add(PM.projectUrlTag, "");
                //SOUNDAR : Need to find license URL or serve it.
                dict.Add(PM.licenseUrlTag, "");
                dict.Add(PM.copyrightTag, "");
                dict.Add(PM.descriptionTag, "");
                dict.Add(PM.releaseNotesTag, "");
                dict.Add(PM.requireLicenseAcceptanceTag, "true");
                dict.Add(PM.summaryTag, msi.SummaryInfo.Comments);
                dict.Add(PM.titleTag, msi.SummaryInfo.Title);
                dict.Add(PM.tagsTag, "msi");
                dict.Add(PM.languagesTag, "en-US");
                dict.Add(PM.ownersTag, "");
                dict.Add(PM.commaseparatedAuthorsTag, msi.SummaryInfo.Author);
                msi.Close();
                return dict;
            }
        }
    }
}