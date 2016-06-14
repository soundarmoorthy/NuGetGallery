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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class GalleryService : IVsIdeService 
    {
        public GalleryService()
        {
            configureCategories();
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

        public IdeCategory[] GetRootCategories(string cultureName)
        {
            return GetAllRootCategories();
        }
        ExtensionCategories categories = new ExtensionCategories();

        private void configureCategories()
        {
            Guid root = new Guid();
            Guid idStudioGallery = categories.Add("Atmel Studio Gallery", "Atmel Studio Gallery", root);
            categories.Add("ASF", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("Debugging", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("Development", "Atmel Studio Gallery", idStudioGallery);
            Guid idDevice = categories.Add("Device", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("ARM", "Device", idDevice);
            categories.Add("AVR", "Device", idDevice);

            categories.Add("Projects", "Atmel Studio Gallery", idStudioGallery);

            Guid idTC = categories.Add("Toolchain", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("GCC", "Toolchain", idTC);
            categories.Add("IAR", "Toolchain", idTC);

            Guid idTools = categories.Add("Tools", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("Debuggers", "Tools", idTools);
            categories.Add("Kits", "Tools", idTools);
            categories.Add("Programmers", "Tools", idTools);
            categories.Add("Simulator", "Tools", idTools);

            Guid idTraining = categories.Add("Training", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("Atmel Training", "Training", idTraining);

            Guid idUtils = categories.Add("Utilities", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("USB Driver", "Utilities", idUtils);

            categories.Add("Verification", "Atmel Studio Gallery", idStudioGallery);
            categories.Add("Wireless", "Atmel Studio Gallery", idStudioGallery);


        }
        public IdeCategory[] GetAllRootCategories()
        {
            return categories.getRootCategories();
        }
        public IdeCategory GetCategoryTree2(Guid categoryId, int level, Dictionary<string, string> requestContext)
        {
            if (categoryId == new Guid())
                return new IdeCategory() { Title = "All"};
            return categories.getCategory(categoryId);
        }

        public Task<IdeCategory> GetCategoryTree2Async(Guid categoryId, int level, Dictionary<string, string> requestContext)
        {
            return null;
        }

        private OrderByEnum getOrderBy(string orderByClause)
        {
            var orderBy = OrderByEnum.Ranking;

            if (orderByClause.Contains("Rating"))
                orderBy = OrderByEnum.Rating;
            if (orderByClause.Contains("LastModified"))
                orderBy = OrderByEnum.LastModified;
            if (orderByClause.Contains("DownloadCount"))
                orderBy = OrderByEnum.DownloadCount;
            if (orderByClause.Contains("Name") || orderByClause.Contains("Title"))
                orderBy = OrderByEnum.Name;
            if (orderByClause.Contains("Author"))
                orderBy = OrderByEnum.Author;
            return orderBy;
        }

        private OrderByDirection getOrderByDirection(string orderByClause)
        {
            var orderByDirection = OrderByDirection.Desc;
            if (orderByClause.Contains(" asc"))
                orderByDirection = OrderByDirection.Asc;
            return orderByDirection;
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
                release.Ratings = new List<Studio.Gallery.Model.ReleaseRating>();
                release.category = package.Tags;
                release.Project.Id = 0;
                release.Project.Description = package.Description;
                release.Project.Title = package.Title;
                release.DownloadCount = package.DownloadCount;
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
            IEnumerable<Model.Release> orderedReleases = null;

            var result = new ReleaseQueryResult();
            var host = OperationContext.Current.IncomingMessageHeaders.To.AbsoluteUri;
            var root = "http://localhost:8080";

            OrderByEnum orderby = getOrderBy(orderByClause);
            if (orderby == OrderByEnum.DownloadCount)
            {
                if (getOrderByDirection(orderByClause) == OrderByDirection.Desc)
                {
                    orderedReleases = releases.OrderByDescending(r => r.DownloadCount);
                }
                else
                {
                    orderedReleases = releases.OrderBy(r => r.DownloadCount);
                }
            }
            else if (orderby == OrderByEnum.Rating )
            {
                if (getOrderByDirection(orderByClause) == OrderByDirection.Desc)
                {
                    orderedReleases = releases.OrderByDescending(r => r.GetAverageRating());
                }
                else
                {
                    orderedReleases = releases.OrderBy(r => r.GetAverageRating());
                }
            }
            else if (orderby == OrderByEnum.Ranking)
            {
                if (getOrderByDirection(orderByClause) == OrderByDirection.Desc)
                {
                    orderedReleases = releases.OrderByDescending(r => r.category);
                }
                else
                {
                    orderedReleases = releases.OrderBy(r => r.category);
                }
            }
            else if (orderby == OrderByEnum.Name)
            {
                if (getOrderByDirection(orderByClause) == OrderByDirection.Desc)
                    orderedReleases = releases.OrderByDescending(r => r.Extension.Name);
                else
                {
                    orderedReleases = releases.OrderBy(r => r.Extension.Name);
                }
            }
            else if (orderby == OrderByEnum.Author)
            {
                if (getOrderByDirection(orderByClause) == OrderByDirection.Desc)
                    orderedReleases = releases.OrderByDescending(r => r.Extension.Author);
                else
                {
                    orderedReleases = releases.OrderBy(r => r.Extension.Author);
                }
            }
            else
            {
                orderedReleases = releases;
            }
            result.Releases = orderedReleases.Select(r => new Galleries.Domain.Model.Release(r, root)).ToArray();

            result.TotalCount = orderedReleases.Count();
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
