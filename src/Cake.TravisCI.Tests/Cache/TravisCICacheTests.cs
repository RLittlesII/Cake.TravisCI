using System;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.TravisCI.Tests.Cache
{
    public sealed class TravisCICacheTests
    {
        public sealed class TheCacheMethod
        {   
            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
            }

            [Fact]
            public void Should_Throw_If_TravisCI_Upload_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("travis: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/travis", "/bin/tools/travis")]
            [InlineData("./tools/travis", "/Working/tools/travis")]
            public void Should_Use_TravisCI_Upload_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_TravisCI_Upload_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/travis", result.Path.FullPath);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("travis: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("travis: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Null()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }
            
            [Fact]
            public void Should_Add_Command()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache", result.Args);
            }
            
            [Fact]
            public void Should_Add_Interactive_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Interactive = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -i", result.Args);
            }
            
            [Fact]
            public void Should_Add_Explode_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.NoExplode = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -E", result.Args);
            }
            
            [Fact]
            public void Should_Add_Skip_Version_Check_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.SkipVersionCheck = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache --skip-version-check", result.Args);
            }
            
            [Fact]
            public void Should_Add_Skip_Completion_Check_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.SkipCompletionCheck = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache --skip-completion-check", result.Args);
            }
            
            [Fact]
            public void Should_Add_Api_Endpoint_Url_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.ApiEndpointUrl = "https://cake.api.com/api";
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -e https://cake.api.com/api", result.Args);
            }
            
            [Fact]
            public void Should_Add_Insecure_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Insecure = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -I", result.Args);
            }
            
            [Fact]
            public void Should_Add_Pro_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Pro = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache --api-endpoint https://api.travis-ci.com/", result.Args);
            }
            
            [Fact]
            public void Should_Add_Org_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Org = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache --api-endpoint https://api.travis-ci.org/", result.Args);
            }
            
            [Fact]
            public void Should_Add_Token_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Token = "bd8f71b4-6080-4011-a95d-48b01504302b";
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -t [REDACTED]", result.Args);
            }
            
            [Fact]
            public void Should_Add_Debug_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Debug = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache --debug", result.Args);
            }
            
            [Fact]
            public void Should_Add_Enterprise_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Enterprise = "starship enterprise";
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -X starship enterprise", result.Args);
            }
            
            [Fact]
            public void Should_Add_Repository_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Repository = "bd8f71b4-6080-4011-a95d-48b01504302b";
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -r bd8f71b4-6080-4011-a95d-48b01504302b", result.Args);
            }
            
            [Fact]
            public void Should_Add_Store_Repository_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.StoreRepository = "bd8f71b4-6080-4011-a95d-48b01504302b";
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -R bd8f71b4-6080-4011-a95d-48b01504302b", result.Args);
            }
            
            [Fact]
            public void Should_Add_Delete_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Delete = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -d", result.Args);
            }
            
            [Fact]
            public void Should_Add_Branch_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Branch = "feature/cake";
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -b feature/cake", result.Args);
            }
            
            [Fact]
            public void Should_Add_Match_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Match = "2.0.0";

                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -m 2.0.0", result.Args);
            }
            
            [Fact]
            public void Should_Add_Force_If_Provided()
            {
                // Given
                var fixture = new TravisCICacheRunnerFixture();
                fixture.Settings.Force = true;
                
                // When
                var result = fixture.Run();
                
                // Then
                Assert.Equal("cache -f", result.Args);
            }
        }
    }
}