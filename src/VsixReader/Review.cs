
namespace VSIXParser
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Release Release { get; set; }

    }
}