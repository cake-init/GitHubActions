public class BuildData
{
    public string Version { get; }
    public string Configuration { get; }
    public DirectoryPath SolutionProjectPath { get; }
    public DotNetMSBuildSettings MSBuildSettings { get; }
    public DotNetRestoreSettings DotNetRestoreSettings { get; }
    public DotNetBuildSettings DotNetBuildSettings { get; }

    public BuildData(
        string version,
        string configuration,
        DirectoryPath solutionProjectPath,
        BuildSystem buildSystem
        )
    {
        Version = version;
        Configuration = configuration;
        SolutionProjectPath = solutionProjectPath;
        MSBuildSettings = new DotNetMSBuildSettings()
                    .SetVersion(version)
                    .SetConfiguration(configuration)
                    .SetContinuousIntegrationBuild(buildSystem.IsRunningOnGitHubActions);
        DotNetRestoreSettings = new DotNetRestoreSettings {
            MSBuildSettings = MSBuildSettings
        };
        DotNetBuildSettings = new DotNetBuildSettings {
            NoRestore = true,
            MSBuildSettings = MSBuildSettings
        };
    }
}