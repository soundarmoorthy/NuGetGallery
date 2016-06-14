namespace Galleries.Domain.Model
{
    public partial class Project
    {
        public Project()
        {

        }

        public Project(Studio.Gallery.Model.Project p)
        {
            Description = p.Description;
            Title = p.Title;
            ModifiedDate = p.ModifiedDate;
            FileReleaseEnabled = true;
            IsPublished = true;
        }
    }
}