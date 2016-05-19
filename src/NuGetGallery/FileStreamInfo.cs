using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NuGetGallery
{
    public class FileStreamInfo : IDisposable
    {

        public readonly string Name;
        public readonly Stream Stream;

	public FileStreamInfo(string filename, Stream stream)
        {
            this.Name = filename;
            this.Stream = stream;
        }

        public void Dispose()
        {
            if (Stream != null)
                Stream.Dispose();
        }
    }
}