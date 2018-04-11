using Cake.Testing.Fixtures;

namespace Cake.TravisCI.Tests
{
    public class TravisCIRunnerFixture : ToolFixture<TravisCISettings>
    {
        public TravisCIRunnerFixture() : base("travis")
        {
        }

        protected override void RunTool()
        {
        }
    }
}