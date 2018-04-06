using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.TravisCI
{
    /// <summary>
    /// <see cref="ToolSettings"/> for TravisCI artifact upload.
    /// </summary>
    /// <remarks> Documentation: https://github.com/travis-ci/artifacts/blob/master/USAGE.md </remarks>
    /// <seealso cref="ToolSettings" />
    public class TravisCIUploadSettings : ToolSettings
    {
        public TravisCIUploadSettings() { EnvironmentVariables = new Dictionary<string, string>(); }
        /// <summary>
        /// Gets or sets the  log output format (text, json, or multiline).
        /// </summary>
        public string LogFormat { get; set; }

        /// <summary>
        /// Gets or sets log level to debug.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets og level to panic.
        /// </summary>
        public bool Quiet { get; set; }

        /// <summary>
        /// Gets or sets the upload credentials key REQUIRED.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets destination bucket REQUIRED.
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// Gets or sets the artifact cache-control header value.
        /// </summary>
        public string CacheControl { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public string Permissions { get; set; }

        /// <summary>
        /// Gets or sets the upload credentials secret REQUIRED.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the S3-region region used when storing to S3.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the repo owner/name slug.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the build number.
        /// </summary>
        public string BuildNumber { get; set; }

        /// <summary>
        /// Gets or sets the build identifier.
        /// </summary>
        public string BuildId { get; set; }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// Gets or sets the job identifier.
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Gets or sets the upload worker concurrency.
        /// </summary>
        public string Concurrency { get; set; }

        /// <summary>
        /// Gets or sets the max combined size of uploaded artifacts.
        /// </summary>
        public string MaxSize { get; set; }

        /// <summary>
        /// Gets or sets the artifact upload provider.
        /// </summary>
        public string UploadProvider { get; set; }

        /// <summary>
        /// Gets or sets the number of upload retries per artifact.
        /// </summary>
        public int? Retries { get; set; }

        /// <summary>
        /// Gets or sets the artifact target paths.
        /// </summary>
        public IEnumerable<FilePath> TargetPaths { get; set; }

        /// <summary>
        /// Gets or sets the artifact save host.
        /// </summary>
        public string SaveHost { get; set; }

        /// <summary>
        /// Gets or sets the artifact save auth token.
        /// </summary>
        public string AuthToken { get; set; }
    }
}