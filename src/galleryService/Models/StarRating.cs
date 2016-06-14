using System;

namespace Studio.Gallery.Models
{
    public class StarRating
    {
        public int ReleaseId { get; set; }
        public int NrStars { get; set; }

        public StarRating(int releaseId, double averageRating)
        {
            ReleaseId = releaseId;
            NrStars = Convert.ToInt32(averageRating);
        }
    }
}