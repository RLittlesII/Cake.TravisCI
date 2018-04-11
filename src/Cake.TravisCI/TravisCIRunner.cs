using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.TravisCI
{
    /// <inheritdoc />
    /// <summary>
    /// Executes the TravisCI command line tool.
    /// </summary>
    /// <seealso cref="T:Cake.Core.Tooling.Tool`1" />
    public class TravisCIRunner : Tool<TravisCISettings>
    {
        /// <inheritdoc />
        public TravisCIRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber) : base(fileSystem, environment, processRunner, globber)
        {
        }

        /// <inheritdoc />
        public TravisCIRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Lists or deletes repository caches.
        /// </summary>
        /// <param name="settings"></param>
        public void Cache(TravisCISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, BuildArguments("cache", settings));
        }

        private ProcessArgumentBuilder BuildArguments(string command, TravisCISettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append(command);

            if(settings.Interactive)
            {
                builder.Append("-i");
            }

            if(settings.NoExplode)
            {
                builder.Append("-E");
            }

            if (settings.SkipVersionCheck)
            {
                builder.Append("--skip-version-check");
            }

            if(settings.SkipCompletionCheck)
            {
                builder.Append("--skip-completion-check");
            }

            if(!string.IsNullOrEmpty(settings.ApiEndpointUrl))
            {
                builder.AppendSwitch("-e", settings.ApiEndpointUrl);
            }

            if(settings.Insecure)
            {
                builder.Append("-I");
            }

            if(settings.Pro)
            {
                builder.Append("--api-endpoint https://api.travis-ci.com/");
            }

            if(settings.Org)
            {
                builder.Append("--api-endpoint https://api.travis-ci.org/");
            }

            if(!string.IsNullOrEmpty(settings.Token))
            {
                builder.AppendSwitchSecret("-t", settings.Token);
            }

            if(settings.Debug)
            {
                builder.Append("--debug");
            }

            if(!string.IsNullOrEmpty(settings.Enterprise))
            {
                builder.AppendSwitch("-X", settings.Enterprise);
            }

            if(!string.IsNullOrEmpty(settings.Repository))
            {
                builder.AppendSwitch("-r", settings.Repository);
            }

            if(!string.IsNullOrEmpty(settings.StoreRepository))
            {
                builder.AppendSwitch("-R", settings.StoreRepository);
            }

            if(settings.Delete)
            {
                builder.Append("-d");
            }

            if(!string.IsNullOrEmpty(settings.Branch))
            {
                builder.AppendSwitch("-b", settings.Branch);
            }

            if(!string.IsNullOrEmpty(settings.Match))
            {
                builder.AppendSwitch("-m", settings.Match);
            }

            if(settings.Force)
            {
                builder.Append("-f");
            }

            return builder.RenderSafe();
        }

        /// <inheritdoc />
        protected override string GetToolName() => "travis";

        /// <inheritdoc />
        protected override IEnumerable<string> GetToolExecutableNames() => new[]{"travis"};
    }
}