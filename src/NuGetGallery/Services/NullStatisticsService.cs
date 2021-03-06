// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NuGetGallery
{
    public class NullStatisticsService : IStatisticsService
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
        public static readonly NullStatisticsService Instance = new NullStatisticsService();

        private NullStatisticsService() { }

        public IEnumerable<StatisticsPackagesItemViewModel> DownloadPackagesSummary
        {
            get { return Enumerable.Empty<StatisticsPackagesItemViewModel>(); }
        }

        public IEnumerable<StatisticsPackagesItemViewModel> DownloadPackageVersionsSummary
        {
            get { return Enumerable.Empty<StatisticsPackagesItemViewModel>(); }
        }

        public IEnumerable<StatisticsPackagesItemViewModel> DownloadPackagesAll
        {
            get { return Enumerable.Empty<StatisticsPackagesItemViewModel>(); }
        }

        public IEnumerable<StatisticsPackagesItemViewModel> DownloadPackageVersionsAll
        {
            get { return Enumerable.Empty<StatisticsPackagesItemViewModel>(); }
        }

        public IEnumerable<StatisticsNuGetUsageItem> NuGetClientVersion
        {
            get { return Enumerable.Empty<StatisticsNuGetUsageItem>(); }
        }

        public IEnumerable<StatisticsWeeklyUsageItem> Last6Weeks
        {
            get { return Enumerable.Empty<StatisticsWeeklyUsageItem>(); }
        }

        public Task<StatisticsReportResult> LoadDownloadPackages()
        {
            return Task.FromResult(StatisticsReportResult.Failed);
        }

        public Task<StatisticsReportResult> LoadDownloadPackageVersions()
        {
            return Task.FromResult(StatisticsReportResult.Failed);
        }

        public Task<StatisticsReportResult> LoadNuGetClientVersion()
        {
            return Task.FromResult(StatisticsReportResult.Failed);
        }

        public Task<StatisticsReportResult> LoadLast6Weeks()
        {
            return Task.FromResult(StatisticsReportResult.Failed);
        }

        public Task<StatisticsPackagesReport> GetPackageDownloadsByVersion(string packageId)
        {
            return Task.FromResult(new StatisticsPackagesReport());
        }

        public Task<StatisticsPackagesReport> GetPackageVersionDownloadsByClient(string packageId, string packageVersion)
        {
            return Task.FromResult(new StatisticsPackagesReport());
        }
    }
}