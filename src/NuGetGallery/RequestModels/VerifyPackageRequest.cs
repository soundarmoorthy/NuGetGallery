﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using NuGet.Packaging;
using NuGet.Versioning;
using System.Collections.Generic;

namespace NuGetGallery
{
    public class VerifyPackageRequest
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string LicenseUrl { get; set; }
        public bool Listed { get; set; }
        public EditPackageVersionRequest Edit { get; set; }
        public NuGetVersion MinClientVersion { get; set; }
        public string Language { get; set; }
        public string DevelopmentDependency { get; set; }
        public DependencySetsViewModel Dependencies { get; set; }
        public IReadOnlyCollection<FrameworkSpecificGroup> FrameworkReferenceGroups { get; set; }
	public string FileType { get; set; }
    }
}