using System.Collections.Generic;
using System.Linq;

namespace Studio.Gallery.Model
{
    public class Release
    {
        public int Id { get; set; }

        public virtual ICollection<ReleaseRating> Ratings { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public int DownloadCount { get; set; }

        public virtual Project Project { get; set; }

        public virtual Extension Extension { get; set; }

        internal void AddRating(int rating)
        {
            Ratings.Add(new ReleaseRating(this, rating));
        }
        public string category { get; set; }


        public Release()
        {
            Ratings = new List<ReleaseRating>();
        }

        public Release(Project project, Extension newExtension)
        {
            Project = project;
            Extension = newExtension;
            DownloadCount = 0;
        }

        public double GetAverageRating()
        {
            if (!Ratings.Any())
                return 0;

            return Ratings.Average(r => r.Rating);
        }
    }
}