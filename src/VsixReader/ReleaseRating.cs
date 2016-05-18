
namespace VSIXParser
{
    public class ReleaseRating
    {
        public int Id { get; set; }
        public int Rating { get; set; }

        public Release Release { get; set; }

        public ReleaseRating()
        {

        }

        public ReleaseRating(Release e, int rating)
        {
            Release = e;
            Rating = rating;
        }
    }
}