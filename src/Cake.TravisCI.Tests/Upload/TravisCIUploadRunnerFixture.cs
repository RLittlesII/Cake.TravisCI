using Cake.Testing.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;
using Cake.TravisCI.Upload;

namespace Cake.TravisCI.Tests.Upload
{
    internal class TravisCIUploadRunnerFixture : ToolFixture<TravisCIUploadSettings>
    {
        public TravisCIUploadRunnerFixture() : base("artifacts")
        {
            Settings.TargetPaths = new List<FilePath>();
        }

        protected override void RunTool()
        {
            var runner = new TravisCIUploadRunner(FileSystem, Environment, ProcessRunner, Tools);
            runner.Upload(Settings);
        }
    }
}
