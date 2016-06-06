// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace NuGetGallery
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IFileStorageService _fileStorageService;

        public UploadFileService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public Task DeleteUploadFileAsync(int userKey)
        {
            return DeleteUploadFileAsync(userKey, Constants.NuGetPackageFileExtension);
        }

        public Task DeleteUploadFileAsync(int userKey, string extension)
        {
            if (userKey < 1)
            {
                throw new ArgumentException(Strings.UserKeyIsRequired, nameof(userKey));
            }

            var uploadFileName = BuildFileName(userKey, extension);

            return _fileStorageService.DeleteFileAsync(Constants.UploadsFolderName, uploadFileName);
        }

        public Task<Stream> GetUploadFileAsync(int userKey)
        {

            return this.GetUploadFileAsync(userKey, Constants.NuGetPackageFileExtension);
        }


        public Task<Stream> GetUploadFileAsync(int userKey, string fileExtension)
        {
            // Use the trick of a private core method that actually does the async stuff to allow for sync arg contract checking
            if (userKey < 1)
            {
                throw new ArgumentException(Strings.UserKeyIsRequired, nameof(userKey));
            }
            return GetUploadFileAsyncCore(userKey, fileExtension);
        }

        public Task SaveUploadFileAsync(int userKey, Stream packageFileStream)
        {
            return this.SaveUploadFileAsync(userKey, packageFileStream, Constants.NuGetPackageFileExtension);
        }

        public Task SaveUploadFileAsync(int userKey, Stream packageFileStream, string extension)
        {
            if (userKey < 1)
            {
                throw new ArgumentException(Strings.UserKeyIsRequired, nameof(userKey));
            }

            if (packageFileStream == null)
            {
                throw new ArgumentNullException(nameof(packageFileStream));
            }

            packageFileStream.Position = 0;

            var uploadFileName = BuildFileName(userKey, extension);
            return _fileStorageService.SaveFileAsync(Constants.UploadsFolderName, uploadFileName, packageFileStream);
        }

        private static string BuildFileName(int userKey, string extension)
        {
            return String.Format(CultureInfo.InvariantCulture, Constants.UploadFileNameTemplate, userKey, extension);
        }

        // Use the trick of a private core method that actually does the async stuff to allow for sync arg contract checking
        private async Task<Stream> GetUploadFileAsyncCore(int userKey,string extension)
        {
            var uploadFileName = BuildFileName(userKey, extension);
            return await _fileStorageService.GetFileAsync(Constants.UploadsFolderName, uploadFileName);
        }

    }
}