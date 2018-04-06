using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.TravisCI.Tests.Upload;
using Xunit;

namespace Cake.TravisCI.Tests
{
    public sealed class TravisCIUploadRunnerTests
    {
        public class TheUploadMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
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
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("artifacts: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/artifacts", "/bin/tools/artifacts")]
            [InlineData("./tools/artifacts", "/Working/tools/artifacts")]
            public void Should_Use_TravisCI_Upload_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
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
                var fixture = new TravisCIUploadRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/artifacts", result.Path.FullPath);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("artifacts: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("artifacts: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Settings_Null()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Theory]
            [InlineData("texzt")]
            [InlineData("json")]
            [InlineData("multiline")]
            public void Should_Add_Log_Information_If_Provided(string format)
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.LogFormat = format;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --log-format {format}", result.Args);
            }

            [Fact]
            public void Should_Add_Debug_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Debug = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --debug", result.Args);
            }

            [Fact]
            public void Should_Add_Quiet_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Quiet = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --quiet", result.Args);
            }

            [Fact]
            public void Should_Add_Key_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Key = Guid.NewGuid().ToString();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --key [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Key_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_KEY", "ARTIFACTS_KEY"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --key [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Bucket_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Bucket = Guid.NewGuid().ToString();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --bucket {fixture.Settings.Bucket}", result.Args);
            }

            [Fact]
            public void Should_Add_Bucket_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_BUCKET", "cake_bucket"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --bucket cake_bucket", result.Args);
            }

            [Fact]
            public void Should_Add_Cache_Control_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.CacheControl = Guid.NewGuid().ToString();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --cache-control [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Cache_Control_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_CACHE_CONTROL", "ARTIFACTS_CACHE_CONTROL"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --cache-control [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Permissions_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Permissions = "admin";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --permissions [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Permissions_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_PERMISSIONS", "ARTIFACTS_PERMISSIONS"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --permissions [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Secret_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Secret = "secret";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --secret [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Secret_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_SECRET", "ARTIFACTS_SECRET"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --secret [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Region_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Region = "N1-Q4";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --s N1-Q4", result.Args);
            }

            [Fact]
            public void Should_Add_Region_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_REGION", "N1-Q4"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --s N1-Q4", result.Args);
            }

            [Fact]
            public void Should_Add_Repository_Slug_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Slug = "slug";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --repo-slug slug", result.Args);
            }

            [Fact]
            public void Should_Add_Repository_Slug_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_REPO_SLUG", "slug"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --repo-slug slug", result.Args);
            }

            [Fact]
            public void Should_Add_Build_Number_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.BuildNumber = "5";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --build-number 5", result.Args);
            }

            [Fact]
            public void Should_Add_Build_Number_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_BUILD_NUMBER", "10"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --build-number 10", result.Args);
            }

            [Fact]
            public void Should_Add_Build_Id_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.BuildId = "5";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --build-id 5", result.Args);
            }

            [Fact]
            public void Should_Add_Build_Id_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_BUILD_ID", "TK421"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --build-id TK421", result.Args);
            }

            [Fact]
            public void Should_Add_Job_Number_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.JobNumber = "5";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --job-number 5", result.Args);
            }

            [Fact]
            public void Should_Add_Job_Number_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_JOB_NUMBER", "10"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --job-number 10", result.Args);
            }

            [Fact]
            public void Should_Add_Job_Id_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.JobId = "5";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --job-id 5", result.Args);
            }

            [Fact]
            public void Should_Add_Job_Id_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_JOB_ID", "10"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --job-id 10", result.Args);
            }

            [Fact]
            public void Should_Add_Concurrency_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Concurrency = "5";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --concurrency 5", result.Args);
            }

            [Fact]
            public void Should_Add_Concurrency_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_CONCURRENCY", "15"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --concurrency 15", result.Args);
            }

            [Fact]
            public void Should_Add_Max_Size_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.MaxSize = "1048576001";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --max-size 1048576001", result.Args);
            }

            [Fact]
            public void Should_Add_Max_Size_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_MAX_SIZE", "1048576001"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --max-size 1048576001", result.Args);
            }

            [Theory]
            [InlineData("artifacts")]
            [InlineData("s3")]
            public void Should_Add_Upload_Provider_If_Provided(string provider)
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.UploadProvider = provider;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --upload-provider {provider}", result.Args);
            }

            [Fact]
            public void Should_Add_Upload_Provider_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_UPLOAD_PROVIDER", "s3"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --upload-provider s3", result.Args);
            }

            [Fact]
            public void Should_Add_Retries_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.Retries = 5;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --retries 5", result.Args);
            }

            [Fact]
            public void Should_Add_Retries_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_RETRIES", "5"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --retries 5", result.Args);
            }

            [Fact]
            public void Should_Add_Target_Paths_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.TargetPaths = new List<FilePath>
                {
                    "./",
                    "./source"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --target-paths \"/Working\":\"/Working/source\"", result.Args);
            }

            [Fact]
            public void Should_Add_Target_Paths_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_TARGET_PATHS", @"./source/cake:./source/travis"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --target-paths ./source/cake:./source/travis", result.Args);
            }

            [Fact]
            public void Should_Add_Working_Directory_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.WorkingDirectory = "./";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --working-dir \"/Working\"", result.Args);
            }

            [Fact]
            public void Should_Add_Working_Directory_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_WORKING_DIR", "./"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --working-dir \"/Working\"", result.Args);
            }

            [Fact]
            public void Should_Add_Save_Host_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.SaveHost = "hostname";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --save-host hostname", result.Args);
            }

            [Fact]
            public void Should_Add_Save_Host_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_SAVE_HOST", "HOST"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --save-host HOST", result.Args);
            }

            [Fact]
            public void Should_Add_Auth_Token_If_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.AuthToken = "token";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"upload --auth-token [REDACTED]", result.Args);
            }

            [Fact]
            public void Should_Add_Auth_Token_If_Environment_Variable_Is_Provided()
            {
                // Given
                var fixture = new TravisCIUploadRunnerFixture();
                fixture.Settings.EnvironmentVariables = new Dictionary<string, string>
                {
                    {"ARTIFACTS_AUTH_TOKEN", "Token"}
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upload --auth-token [REDACTED]", result.Args);
            }
        }
    }
}
