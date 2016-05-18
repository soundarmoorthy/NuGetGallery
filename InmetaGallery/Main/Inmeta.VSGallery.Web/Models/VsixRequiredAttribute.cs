using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace Inmeta.VSGallery.Web.Models
{
    public class ValidExtensionRequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var file = value as HttpPostedFileBase;

            return file != null;
        }

    }
}