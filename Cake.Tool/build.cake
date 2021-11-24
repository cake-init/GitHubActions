#load "build/BuildData.cake"

Setup(context => new BuildData(
                    "1.0.0",
                    "Release",
                    context.MakeAbsolute(Directory("./src")),
                    BuildSystem));

Task("Clean")
    .Does<BuildData>(static (context, data) => {
        context.CleanDirectories("./**/{{bin,obj}}/" + data.Configuration);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does<BuildData>(static (context, data) => {
        context.DotNetRestore(data.SolutionProjectPath.FullPath,
            data.DotNetRestoreSettings);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does<BuildData>(static (context, data) => {
        context.DotNetBuild(data.SolutionProjectPath.FullPath,
            data.DotNetBuildSettings);
});

var defaultTask = Task("Default")
                    .IsDependentOn("Clean")
                    .IsDependentOn("Restore")
                    .IsDependentOn("Build");
var gitHubActionsTask = Task(nameof(BuildProvider.GitHubActions))
                            .IsDependentOn(defaultTask);


RunTarget(
    Argument(
        "target",
        BuildSystem.IsRunningOnGitHubActions
            ? gitHubActionsTask.Task.Name
            : defaultTask.Task.Name
    )
);