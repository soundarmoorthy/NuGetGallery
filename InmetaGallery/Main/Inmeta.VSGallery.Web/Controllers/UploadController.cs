using System;
using System.Linq;
using System.Web.Mvc;
using Inmeta.VSGallery.Model;
using Inmeta.VSGallery.Web.Models;
using Inmeta.VSIX;
using System.IO;
using VSIXParser;

namespace Inmeta.VSGallery.Web.Controllers
{
    public partial class UploadController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Post(UploadExtensionModel model)
        {
            if (ModelState.IsValid)
            {

                using (var ctx = new GalleryContext())
                {
                    var filename = model.File;
                    var fileExt = Path.GetExtension(filename.FileName);

                    Extension extension = null;
                    if (fileExt == ".vsix")
                        extension = HandleVsix(model, ctx);
                    else
                        extension = HandleNonVsix(model, ctx);

                    ctx.SaveChanges();
                    return RedirectToAction(MVC.Extension.Index(extension.VsixId));
                }
            }
            return View(Views.Index);
        }

        private static Extension HandleVsix(UploadExtensionModel model, GalleryContext ctx)
        {
            var vsix = model.ReadFileContent();
            var vsixItem = VsixRepository.Read(vsix, model.File.FileName);
            var extension = ctx.ExtensionsWithStuff.FirstOrDefault(e => e.VsixId == vsixItem.VsixId);
            if (extension == null)
            {
                extension = new Extension(vsixItem, vsix);
                var project = new Project(extension.Name, extension.Description);
                var release = new Release(project, extension);
                ctx.Releases.Add(release);
            }
            else
            {
                extension.Update(vsixItem, vsix);
                extension.Release.Project.ModifiedDate = DateTime.Now;
            }

            return extension;
        }

        private Extension HandleNonVsix(UploadExtensionModel model, GalleryContext ctx)
        {
            var nonVsix = model.ReadFileContent();
            var vsixItem = GetInputFromUI(nonVsix, Path.GetFileNameWithoutExtension(model.File.FileName));
            var extension = ctx.ExtensionsWithStuff.FirstOrDefault(e => e.VsixId == vsixItem.VsixId);
            if (extension == null)
            {
                extension = new Extension(vsixItem, nonVsix);
                var project = new Project(extension.Name, extension.Description);
                var release = new Release(project, extension);
                ctx.Releases.Add(release);
            }
            else
            {
                extension.Update(vsixItem, nonVsix);
                extension.Release.Project.ModifiedDate = DateTime.Now;
            }

            return extension;
        }

        private VsixItem GetInputFromUI(byte[] msi, string name)
        {
            Vsix v = new Vsix();
            v.Version = "1.0";
            v.Identifier = new VsixIdentifier()
            {
                Author = "Soundararajan Dhakshinamoorthy",
                AllUsers = true,
                Description = "This is a msi file and the description will have to be manually handled",
                Id = Guid.NewGuid().ToString(),
                InstalledByMsi = true,
                License = "This file is unlicensed",
                MoreInfoUrl = "http://dhakshinamoorthy.wordpress.com/about",
                Name = name,
                Version = "100.0",
                InstalledByMsiSpecified = true,
                AllUsersSpecified = true,
                GettingStartedGuide = "http://dhakshinamoorthy.wordpress.com"
            };
            var vsixItem = new VsixItem(v, msi);
            return vsixItem;
        }
    }
}