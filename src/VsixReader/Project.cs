using System;

namespace VSIXParser
{
    public class Project
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Project()
        {

        }
        public Project(string title, string description)
        {
            Title = title;
            Description = description;
            ModifiedDate = DateTime.Now;
        }

    }
}