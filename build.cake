
/* #region  Dependencies */
using System.Diagnostics;
using System.Linq;
/* #endregion Dependencies */

/* #region  Addins */
#addin nuget:?package=Cake.Git&version=0.21.0
#addin nuget:?package=Cake.Npm&version=0.17.0
#addin nuget:?package=Cake.FileHelpers&version=3.2.0
/* #endregion Addins */

/* #region  Tools */
#tool nuget:?package=GitVersion.CommandLine&version=5.0.1
#tool nuget:?package=vswhere&version=2.3.7
/* #endregion Tools */

/* #region  Arguments */
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var baseApiUrl = Argument("baseApiUrl", "http://localhost:8100");
/* #endregion Arguments */

/* #region  Variables */
// GitVersion gitVersion;
// var isMasterBranch = gitVersion.BranchName.StartsWith("main");
/* #endregion Variables */

/* #region  Functions */
ProcessArgumentBuilder AddVersionArg(ProcessArgumentBuilder args) {
  // args.Append($"/p:Version={gitVersion.NuGetVersionV2}");
  return args;
}
/* #endregion Functions */

// Task("GitVersion").Does(() => {
//   gitVersion = GitVersion();
// });

/* #region  Autorest */
Task("InstallAutorest").Does(() => {
  NpmInstall(settings => settings.AddPackage("autorest", "3.1.0").InstallGlobally());
});

Task("Swagger").Does(() => {
  DotNetBuild("WWA.RestApi/WWA.RestApi.csproj", settings => settings
    .SetConfiguration(configuration)
    .SetMaxCpuCount(0)
    .SetVerbosity(Verbosity.Minimal)
  // .WithWarningsAsError()
  // .WithWarningsAsMessage(new [] { "CS3021" })
    .WithProperty("GenerateSwagger", new string[] { "true" })
  );
});

Task("Autorest")
  .IsDependentOn("Swagger")
  .IsDependentOn("InstallAutorest")
.Does(() => {
    var args = new ProcessArgumentBuilder()
      .Append("/c autorest.cmd")
      .Append("--v3")
      .Append($"--input-file=WWA.RestApi/wwwroot/docs/swashbuckle/spec/v1.json")
      .Append("--csharp")
      .Append("--output-folder=WWA.RestApi.CsharpClient/GeneratedCode")
      .Append("--namespace=WWA.RestApi.CsharpClient")
      .Append($"--override-client-name=WwaRestApiClient")
      .Append("--clear-output-folder")
      .Append("--version=latest")
      .Append("--sync-methods=none")
      .Append("--markOpenAPI3ErrorsAsWarning")
      .Append("--legacy");

    var proc = new Process {
      StartInfo = new ProcessStartInfo("cmd", args.Render()) {
        UseShellExecute = false,
        RedirectStandardError = true,
        CreateNoWindow = true
      }
    };

    Information($"Generating csharp autorest client...");
    proc.Start();
    proc.WaitForExit();
    if (proc.ExitCode != 0) {
      Error("Failed to generate csharp autorest client with the following errors:");
      while (!proc.StandardError.EndOfStream) {
        Error(proc.StandardError.ReadLine());
      }
    }
  }
);
/* #endregion Autorest */

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
