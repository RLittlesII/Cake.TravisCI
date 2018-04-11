using Cake.Testing.Fixtures;

namespace Cake.TravisCI.Tests.Cache
{
    internal class TravisCICacheRunnerFixture : ToolFixture<TravisCISettings>
    {
        public TravisCICacheRunnerFixture() : base("travis")
        {
        }

        protected override void RunTool()
        {
            var runner = new TravisCIRunner(FileSystem, Environment, ProcessRunner, Tools);
            runner.Cache(Settings);
        }
    }
}