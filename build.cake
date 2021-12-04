
/* #region  Dependencies */
using System.Diagnostics;
using System.Linq;
/* #endregion Dependencies */

/* #region  Addins */
#addin nuget:?package=Cake.Git&version=0.21.0
#addin nuget:?package=Cake.Npm&version=0.17.0
#addin nuget:?package=Cake.FileHelpers&version=3.2.0
#addin nuget:?package=Cake.Incubator
/* #endregion Addins */

/* #region  Tools */
#tool "dotnet:?package=GitVersion.Tool"
#tool nuget:?package=vswhere&version=2.3.7
/* #endregion Tools */

/* #region Variables */
GitVersion gitVersion;
/* #endregion Variables */

/* #region  Arguments */
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var baseApiUrl = Argument("baseApiUrl", "http://localhost:8100");
/* #endregion Arguments */

Task("GitVersion").Does(() => {
  gitVersion = GitVersion();
  Console.WriteLine(gitVersion.Dump());
});

/* #region  Autorest */
Task("InstallAutorest").Does(() => {
  NpmInstall(settings => settings.AddPackage("autorest", "3.1.0").InstallGlobally());
});

Task("Swagger").Does(() => {
  var settings = new DotNetBuildSettings
  {
    Verbosity = DotNetVerbosity.Minimal,
    EnvironmentVariables = new Dictionary<string,string>
    {
      ["GenerateSwagger"] = "true"
    }
  };
  DotNetBuild("WWA.RestApi/WWA.RestApi.csproj", settings);
});

Task("Autorest")
  .IsDependentOn("GitVersion")
  .IsDependentOn("Swagger")
  .IsDependentOn("InstallAutorest")
.Does(() => {
    var args = new ProcessArgumentBuilder()
      .Append("/c autorest.cmd")
      .Append("--v3")
      .Append($"--input-file=./WWA.RestApi/wwwroot/docs/swashbuckle/spec/v1.json")
      .Append("--csharp")
      .Append("--output-folder=./WWA.RestApi.CsharpClient/GeneratedCode")
      .Append("--namespace=WWA.RestApi.CsharpClient")
      .Append($"--override-client-name=WwaRestApiClient")
      .Append("--clear-output-folder")
      .Append("--version=latest")
      .Append("--sync-methods=none")
      .Append("--markOpenAPI3ErrorsAsWarning")
      .Append("--legacy")
      .Append($"/p:Version={gitVersion.NuGetVersionV2}");

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
    
    args = new ProcessArgumentBuilder()
      .Append("/c autorest.cmd")
      .Append("--v3")
      .Append($"--input-file=./WWA.RestApi/wwwroot/docs/swashbuckle/spec/v1.json")
      .Append("--typescript")
      .Append("--output-folder=./WWA.RestApi.TypescriptClient/GeneratedCode")
      .Append("--namespace=WWA.RestApi.TypescriptClient")
      .Append($"--override-client-name=WwaRestApiClient")
      .Append("--clear-output-folder")
      .Append("--version=latest")
      .Append("--sync-methods=none")
      .Append("--markOpenAPI3ErrorsAsWarning")
      .Append("--legacy")
      .Append($"/p:Version={gitVersion.NuGetVersionV2}");

    proc = new Process {
      StartInfo = new ProcessStartInfo("cmd", args.Render()) {
        UseShellExecute = false,
        RedirectStandardError = true,
        CreateNoWindow = true
      }
    };

    Information($"Generating typescript autorest client...");
    proc.Start();
    proc.WaitForExit();
    if (proc.ExitCode != 0) {
      Error("Failed to generate typescript autorest client with the following errors:");
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
