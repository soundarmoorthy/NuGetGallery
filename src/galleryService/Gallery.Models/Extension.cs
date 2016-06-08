using System.Collections.Generic;

namespace Studio.Gallery.Model
{
    public class Extension
    {
        public int Id { get; set; }
        public string VsixId { get; set; }
        public string VsixVersion { get; set; }
        public string Tool { get; set; }
        public string Author { get; set; }
        public string PreviewImage { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MoreInfo { get; set; }
        public virtual byte[] Content { get; set; }

        public virtual byte[] IconContent { get; set; }
        public virtual byte[] PreviewImageContent { get; set; }
        public Release Release { get; set; }
        public string ManifestVersion { get; set; }

        public string DownloadUrl(string baseUrl)
        {
            return baseUrl + "/api/v2/package/" + VsixId;
        }

        public Extension()
        {
        }
        public Dictionary<string, string> VsixMetadata(string baseUrl)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            temp["VsixId"] = VsixId;
            temp["VsixVersion"] = VsixVersion;
            temp["Type"] = Tool;
            temp["DownloadUrl"] = DownloadUrl(baseUrl);
            temp["DownloadUpdateUrl"] = DownloadUrl(baseUrl);
            temp["Author"] = Author;
            temp["PreviewImage"] = PreviewImage;
            temp["Icon"] = baseUrl + "/api/Icon?vsixId=" + VsixId;
            return temp;
                //return new Dictionary<string, string>
                //{
                //    {"VsixId", VsixId},
                //    {"VsixVersion", VsixVersion},
                //    {"Type", "Tool"},
                //    {"DownloadUrl", DownloadUrl(baseUrl) },
                //    {"DownloadUpdateUrl", DownloadUrl(baseUrl)},
                //    {"Author", Author},
                //    {"PreviewImage", baseUrl + "/api/PreviewImage?vsixId=" + VsixId},
                //    {"Icon", baseUrl + "/api/Icon?vsixId=" + VsixId }
                //};
        }

        public void AddRating(int rating)
        {
            if (rating != 0)
            {
                Release.Ratings.Add(new ReleaseRating(Release, rating));
            }
        }

        public void IncreaseDownloadCount()
        {
            Release.DownloadCount += 1;
        }
    }
}