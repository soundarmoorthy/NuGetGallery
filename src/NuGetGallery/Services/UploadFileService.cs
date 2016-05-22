﻿// Copyright (c) .NET Foundation. All rights reserved.
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
            if (userKey < 1)
            {
                throw new ArgumentException(Strings.UserKeyIsRequired, nameof(userKey));
            }

            var uploadFileName = BuildFileName(userKey);

            return _fileStorageService.DeleteFileAsync(Constants.UploadsFolderName, uploadFileName);
        }

        public Task<FileStreamContext> GetUploadFileAsync(int userKey)
        {
            if (userKey < 1)
            {
                throw new ArgumentException(Strings.UserKeyIsRequired, nameof(userKey));
            }

            // Use the trick of a private core method that actually does the async stuff to allow for sync arg contract checking
            return GetUploadFileAsyncCore(userKey);
        }

        public Task SaveUploadFileAsync(int userKey, Stream packageFileStream)
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

            var uploadFileName = BuildFileName(userKey);
            return _fileStorageService.SaveFileAsync(Constants.UploadsFolderName, uploadFileName, packageFileStream);
        }

        private static string BuildFileName(int userKey)
        {
            return String.Format(CultureInfo.InvariantCulture, Constants.UploadFileNameTemplate, userKey, Constants.VsixPackageFileExtension);
        }

        // Use the trick of a private core method that actually does the async stuff to allow for sync arg contract checking
        private async Task<FileStreamContext> GetUploadFileAsyncCore(int userKey)
        {
            var uploadFileName = BuildFileName(userKey);
            var stream = await _fileStorageService.GetFileAsync(Constants.UploadsFolderName, uploadFileName);
            var name = string.Empty;
            if (stream != null)
                name = ((FileStream)stream).Name; 
            return new FileStreamContext(name, stream);
        }
    }
}