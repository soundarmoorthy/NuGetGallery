using Galleries.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio.Gallery
{
    class ExtensionCategories
    {
        public Dictionary<int, IdeCategory> roots = new Dictionary<int, IdeCategory>();
        public int index = 0;

        Dictionary<IdeCategory, List<IdeCategory>> categories = new Dictionary<IdeCategory, List<IdeCategory>>();
        List<IdeCategory> allCategories = new List<IdeCategory>();
        public Guid Add(string title, string parentTitle,Guid idParent)
        {
            IdeCategory cat = new IdeCategory();
            cat.Title = title;
            cat.Id = Guid.NewGuid();

            IdeCategory catParent = new IdeCategory();
            catParent.Title = parentTitle;
            catParent.Id = idParent;
            cat.Parent = catParent;
            List<IdeCategory> cats = new List<IdeCategory>();
            if (categories.TryGetValue(catParent, out cats))
            {
                cats.Add(cat);
            }
            else
            {
                categories[catParent] = new List<IdeCategory>();
                categories[catParent].Add(cat);
            }
            allCategories.Add(cat);
            return cat.Id;
        }

        public IdeCategory[] getCategories()
        {
            return allCategories.ToArray();
        }

        public IdeCategory getCategory(Guid id)
        {
            foreach(var cat in allCategories)
            {
                if (cat.Parent.Id == id)
                    return cat.Parent;
            }
            return null;
        }

        public IdeCategory[] getRootCategories()
        {
            List<IdeCategory> roots = new List<IdeCategory>();
            foreach (var cat in categories)
            {
                if( ! roots.Exists(element => element.Title == cat.Key.Title) )
                    roots.Add(cat.Key);
            }
            return roots.ToArray();
        }
    }
}
