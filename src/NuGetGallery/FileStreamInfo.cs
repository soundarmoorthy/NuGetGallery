using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NuGetGallery
{
    public class FileStreamContext : IDisposable
    {

        public readonly string Name;
        public readonly Stream Stream;

	public string Username { get; set; }

	public FileStreamContext(string filename, Stream stream)
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