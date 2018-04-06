using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.TravisCI.Upload
{
    public class TravisCIUploadRunner : Tool<TravisCIUploadSettings>
    {
        private readonly ICakeEnvironment _environment;

        public TravisCIUploadRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber) : base(fileSystem, environment, processRunner, globber)
        {
        }

        public TravisCIUploadRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        public void Upload(TravisCIUploadSettings settings) => Run(settings, BuildArguments(settings));

        private ProcessArgumentBuilder BuildArguments(TravisCIUploadSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var builder = new ProcessArgumentBuilder();

            builder.Append("upload");

            if (settings.LogFormat != null)
            {
                builder.AppendSwitch("--log-format", settings.LogFormat);
            }

            if (settings.Debug)
            {
                builder.Append("--debug");
            }

            if (settings.Quiet)
            {
                builder.Append("--quiet");
            }

            if (!string.IsNullOrEmpty(settings.Key))
            {
                builder.AppendSwitchSecret("--key", settings.Key);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_KEY", out string key))
            {
                builder.AppendSwitchSecret("--key", key);
            }

            if (!string.IsNullOrEmpty(settings.Bucket))
            {
                builder.AppendSwitch("--bucket", settings.Bucket);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_BUCKET", out string bucket))
            {
                builder.AppendSwitch("--bucket", bucket);
            }

            if (!string.IsNullOrEmpty(settings.CacheControl))
            {
                builder.AppendSwitchSecret("--cache-control", settings.CacheControl);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_CACHE_CONTROL", out string cache))
            {
                builder.AppendSwitchSecret("--cache-control", cache);
            }

            if (!string.IsNullOrEmpty(settings.Permissions))
            {
                builder.AppendSwitchSecret("--permissions", settings.Permissions);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_PERMISSIONS", out string permissions))
            {
                builder.AppendSwitchSecret("--permissions", permissions);
            }

            if (!string.IsNullOrEmpty(settings.Secret))
            {
                builder.AppendSwitchSecret("--secret", settings.Secret);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_SECRET", out string secret))
            {
                builder.AppendSwitchSecret("--secret", secret);
            }

            if (!string.IsNullOrEmpty(settings.Region))
            {
                builder.AppendSwitch("--s", settings.Region);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_REGION", out string region))
            {
                builder.AppendSwitch("--s", region);
            }

            if (!string.IsNullOrEmpty(settings.Slug))
            {
                builder.AppendSwitch("--repo-slug", settings.Slug);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_REPO_SLUG", out string slug))
            {

                builder.AppendSwitch("--repo-slug", slug);
            }

            if (!string.IsNullOrEmpty(settings.BuildNumber))
            {
                builder.AppendSwitch("--build-number", settings.BuildNumber);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_BUILD_NUMBER", out string buildNumber))
            {
                builder.AppendSwitch("--build-number", buildNumber);
            }

            if (!string.IsNullOrEmpty(settings.BuildId))
            {
                builder.AppendSwitch("--build-id", settings.BuildId);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_BUILD_ID", out string buildId))
            {
                builder.AppendSwitch("--build-id", buildId);
            }

            if (!string.IsNullOrEmpty(settings.JobNumber))
            {
                builder.AppendSwitch("--job-number", settings.JobNumber);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_JOB_NUMBER", out string jobNumber))
            {
                builder.AppendSwitch("--job-number", jobNumber);
            }

            if (!string.IsNullOrEmpty(settings.JobId))
            {
                builder.AppendSwitch("--job-id", settings.JobId);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_JOB_ID", out string jobId))
            {
                builder.AppendSwitch("--job-id", jobId);
            }

            if (!string.IsNullOrEmpty(settings.Concurrency))
            {
                builder.AppendSwitch("--concurrency", settings.Concurrency);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_CONCURRENCY", out string concurrency))
            {
                builder.AppendSwitch("--concurrency", concurrency);
            }

            if (!string.IsNullOrEmpty(settings.MaxSize))
            {
                builder.AppendSwitch("--max-size", settings.MaxSize);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_MAX_SIZE", out string size))
            {
                builder.AppendSwitch("--max-size", size);
            }

            if (!string.IsNullOrEmpty(settings.UploadProvider))
            {
                builder.AppendSwitch("--upload-provider", settings.UploadProvider);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_UPLOAD_PROVIDER", out string provider))
            {
                builder.AppendSwitch("--upload-provider", provider);
            }

            if (settings.Retries != null)
            {
                builder.AppendSwitch("--retries", settings.Retries.ToString());
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_RETRIES", out string retries))
            {
                builder.AppendSwitch("--retries", retries);
            }

            if (settings.TargetPaths.Any())
            {
                var paths = string.Join(":",
                    settings.TargetPaths.Select(x => $"\"{x.MakeAbsolute(_environment).FullPath}\""));
                builder.AppendSwitch("--target-paths", paths);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_TARGET_PATHS", out string paths))
            {
                builder.AppendSwitch("--target-paths", paths);
            }

            if (settings.WorkingDirectory != null)
            {
                builder.AppendSwitchQuoted("--working-dir",
                    settings.WorkingDirectory.MakeAbsolute(_environment).FullPath);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_WORKING_DIR", out string directory))
            {
                builder.AppendSwitchQuoted("--working-dir",
                    ((FilePath)directory).MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrEmpty(settings.SaveHost))
            {
                builder.AppendSwitch("--save-host", settings.SaveHost);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_SAVE_HOST", out string host))
            {
                builder.AppendSwitch("--save-host", host);
            }

            if (!string.IsNullOrEmpty(settings.AuthToken))
            {
                builder.AppendSwitchSecret("--auth-token", settings.AuthToken);
            }
            else if (settings.EnvironmentVariables.TryGetValue("ARTIFACTS_AUTH_TOKEN", out string authToken))
            {
                builder.AppendSwitchSecret("--auth-token", authToken);
            }

            return builder.RenderSafe();
        }

        protected override string GetToolName() => "artifacts";

        protected override IEnumerable<string> GetToolExecutableNames() => new[] {"artifacts"};
    }
}
