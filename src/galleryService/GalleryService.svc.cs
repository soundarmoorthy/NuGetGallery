using System;

using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Galleries.Domain.Model;
using VsGallery.WebServices;
using Release = Galleries.Domain.Model.Release;
using NuGet;

namespace Studio.Gallery
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GalleryService : IVsIdeService 
    {
        public GalleryService()
        {
        }
        public IdeCategory GetCategoryTree(
            Guid categoryId,
            int level,
            string projectType,
            string templateType,
            string[] skus,
            string[] subSkus,
            int[] templateGroupIds,
            int[] vsVersions,
            string cultureName)
        {
            return null;
        }

        public Task<IdeCategory> GetCategoryTreeAsync(
            Guid categoryId,
            int level,
            string projectType,
            string templateType,
            string[] skus,
            string[] subSkus,
            int[] templateGroupIds,
            int[] vsVersions,
            string cultureName)
        {
            return null;
        }

        public IdeCategory[] GetRootCategories(string cultureName)
        {
            return new[]
                   {
                       new IdeCategory { Title = "Toolchains" }, new IdeCategory {Title="Utilities"},new IdeCategory {Title = "Software Libraries"}
                   };
        }

        public Task<IdeCategory[]> GetRootCategoriesAsync(string cultureName)
        {
            return null;
        }

        public ReleaseQueryResult SearchReleases(
            string searchText,
            string whereClause,
            string orderByClause,
            int? locale,
            int? skip,
            int? take)
        {
            return null;
        }

        public Task<ReleaseQueryResult> SearchReleasesAsync(string searchText, string whereClause, string orderByClause, int? locale, int? skip, int? take)
        {
            return null;
        }

        public IdeCategory[] GetRootCategories2(Dictionary<string, string> requestContext)
        {
            return GetRootCategories(null);
        }

        public Task<IdeCategory[]> GetRootCategories2Async(Dictionary<string, string> requestContext)
        {
            return null;
        }

        public IdeCategory GetCategoryTree2(Guid categoryId, int level, Dictionary<string, string> requestContext)
        {
            return null;
        }

        public Task<IdeCategory> GetCategoryTree2Async(Guid categoryId, int level, Dictionary<string, string> requestContext)
        {
            return null;
        }

        public ReleaseQueryResult SearchReleases2(
            string searchText,
            string whereClause,
            string orderByClause,
            int? skip,
            int? take,
            Dictionary<string, string> requestContext)        
        {
            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("http://localhost:8080/api/v2/");
            var packages = repo.GetPackages();
            var releases = new List<Studio.Gallery.Model.Release>();

            foreach (var pack in packages)
            {
                DataServicePackage package = (NuGet.DataServicePackage)pack;
                var release = new Studio.Gallery.Model.Release();
                release.Project = new Studio.Gallery.Model.Project();
                release.Reviews = new List<Studio.Gallery.Model.Review>();

                release.Project.Id = 0;
                release.Project.Description = package.Description;
                release.Project.Title = package.Title;

                release.Extension = new Studio.Gallery.Model.Extension();
                release.Extension.VsixId = package.Id;
                release.Extension.Author = package.Authors;
                release.Extension.Description = package.Description;
                release.Extension.Icon = package.IconUrl == null ? string.Empty : package.IconUrl.ToString();
                release.Extension.PreviewImage = package.IconUrl == null ? string.Empty : package.IconUrl.ToString();
                release.Extension.VsixVersion = package.Version.ToString();
                release.Extension.Name = package.Title;
                release.Extension.MoreInfo = package.Description;
                
                releases.Add(release);
            }

            var result = new ReleaseQueryResult();
            var host = OperationContext.Current.IncomingMessageHeaders.To.AbsoluteUri;
            //var root = host.Replace("GalleryService.svc", "");
            var root = "http://localhost:8080";

            result.Releases = releases.Select(r => new Galleries.Domain.Model.Release(r, root)).ToArray();

            result.TotalCount = releases.Count();
            return result;
        }

        private static string ParseVsixId(string whereClause)
        {
            string vsixid = null;
            Match match = Regex.Match(whereClause, @"Project\.Metadata\['VsixID'\] = '(?<vsixid>.*?)'", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                vsixid = match.Groups["vsixid"].Value;
            }
            return vsixid;
        }

        public Task<ReleaseQueryResult> SearchReleases2Async(
            string searchText,
            string whereClause,
            string orderByClause,
            int? skip,
            int? take,
            Dictionary<string, string> requestContext)
        {
            return null;
        }

        public string[] GetCurrentVersionsForVsixList(string[] vsixIds, Dictionary<string, string> requestContext)
        {
            return null;
        }

        public Task<string[]> GetCurrentVersionsForVsixListAsync(string[] vsixIds, Dictionary<string, string> requestContext)
        {
            return null;
        }
    }
}
