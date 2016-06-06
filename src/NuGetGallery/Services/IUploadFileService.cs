// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.IO;
using System.Threading.Tasks;

namespace NuGetGallery
{
    public interface IUploadFileService
    {
        Task DeleteUploadFileAsync(int userKey);
        Task DeleteUploadFileAsync(int userKey, string extension);

        Task<Stream> GetUploadFileAsync(int userKey);
        Task<Stream> GetUploadFileAsync(int userKey, string fileExtension);

        Task SaveUploadFileAsync(int userKey, Stream packageFileStream);
        Task SaveUploadFileAsync(int userKey, Stream packageFileStream, string extension);
    }
}