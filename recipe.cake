#load nuget:package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context, 
                            title: "Cake.TravisCI",
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            repositoryOwner: "cake-contrib",  
                            repositoryName: "Cake.TravisCI",  
                            appVeyorAccountName: "cakecontrib",
                            shouldRunDupFinder: false,
                            shouldRunCodecov: false,
                            shouldRunDotNetCorePack: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/src/Cake.TravisCI.Tests/**/*.cs", BuildParameters.RootDirectoryPath + "/src/**/Cake.TravisCI.AssemblyInfo.cs"  }, 
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ", 
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*", 
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();
