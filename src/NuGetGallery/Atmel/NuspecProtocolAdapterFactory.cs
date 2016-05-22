using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery
{
    public static class NuspecProtocolAdapterFactory
    {
        public static INuspecProtocolAdapter Create(string fileType)
        {
	     switch(fileType)
            {
                case Constants.VsixPackageFileExtension:
                    return new VsixNuspecProtocolAdapter();

                case Constants.MsiPackageFileExtension:
                    return new MsiNuspecProtocolAdapter();

                case Constants.ExePackageFileExtension:
                    return new ExeNuspecProtocolAdapter();
                default:
                    throw new InvalidOperationException("Unknown file type");

            }
        }
    }
}