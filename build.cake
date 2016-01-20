#addin nuget:?package=Cake.AppVeyor
#addin nuget:?package=Cake.Yaml

var TARGET = Argument ("target", Argument ("t", "Default"));

var APPVEYOR_APITOKEN = EnvironmentVariable ("APPVEYOR_APITOKEN") ?? "";
var APPVEYOR_ACCOUNTNAME = EnvironmentVariable ("APPVEYOR_ACCOUNTNAME") ?? "JamesMontemagno";
var APPVEYOR_PROJECTSLUG = EnvironmentVariable ("APPVEYOR_PROJECTSLUG") ?? "xamarin-plugins";
var APPVEYOR_BUILD_NUMBER = EnvironmentVariable ("APPVEYOR_BUILD_NUMBER") ?? "9999";

var COMMIT = EnvironmentVariable ("APPVEYOR_REPO_COMMIT") ?? "";
var GIT_PATH = EnvironmentVariable ("GIT_EXE") ?? (IsRunningOnWindows () ? "git.exe" : "git");

var PROJECTS = DeserializeYamlFromFile<List<Project>> ("./projects.yaml");

Func<FilePath> GetCakeToolPath = () =>
{
	var possibleExe = GetFiles ("../**/tools/Cake/Cake.exe").FirstOrDefault ();

	if (possibleExe != null)
		return possibleExe;
		
	var p = System.Diagnostics.Process.GetCurrentProcess ();	
	return new FilePath (p.Modules[0].FileName);
};

public class Project 
{
	public Project () 
	{
		Name = string.Empty;
		BuildScript = string.Empty;
		TriggerPaths = new List<string> ();
		BuildTargets = new List<string> ();
	}

	public string Name { get; set; }
	public string BuildScript { get; set; }
	public List<string> TriggerPaths { get; set; }
	public List<string> BuildTargets { get; set; }
	public string Version { get; set; }

	public override string ToString ()
	{
		return Name;
	}
}

Task ("Default").Does (() =>
{
	var lastSuccessfulCommit = AppVeyorProjectLastSuccessfulBuild (
		new AppVeyorSettings { ApiToken = APPVEYOR_APITOKEN },
		"JamesMontemagno",
		"xamarin-plugins",
		null, null).Build.CommitId;

	Information ("GIT_COMMIT: {0}", COMMIT);
	Information ("GIT_PREV_COMMIT: {0}", lastSuccessfulCommit);
	
	// Get all the changed files in this commit
	IEnumerable<string> changedFiles = new List<string> ();

	// Get files changed in commit range
	if (!string.IsNullOrWhiteSpace (lastSuccessfulCommit)) {
		// We have both commit hashes (previous and current) so do a diff on them
		StartProcess (GIT_PATH, new ProcessSettings { 
			Arguments = "--no-pager diff --name-only " + lastSuccessfulCommit + " " + COMMIT,
			RedirectStandardOutput = true,
		}, out changedFiles);
	} else {
		// We only have the current commit hash, so list files for this commit only
		StartProcess (GIT_PATH, new ProcessSettings { 
			Arguments = "--no-pager show --pretty=\"format:\" --name-only " + COMMIT,
			RedirectStandardOutput = true,
		}, out changedFiles);
	}

	var projectsToBuild = new List<Project> ();

	// Find all the projects that had changed files in their trigger paths
	// and add them to the list to build
	foreach (var file in changedFiles) {
		Information ("\t\tChanged File: {0}", file);
		foreach (var project in PROJECTS) {
			foreach (var triggerPath in project.TriggerPaths) {
				if (file.StartsWith (triggerPath.ToString ())) {
					Information ("\t\tMatched: {0}", triggerPath);
					if (!projectsToBuild.Contains (project))
						projectsToBuild.Add (project);
					break;
				}
			}
		}
	}

	// Now go through all the projects to build and build them
	foreach (var project in projectsToBuild) {
		var buildVersion = project.Version.Replace ("{build}", APPVEYOR_BUILD_NUMBER);

		Information ("\tBuilding: {0} ({1})", project.BuildScript, buildVersion);
		// Build each target specified in the manifest
		foreach (var target in project.BuildTargets) {
			var cakeSettings = new CakeSettings { 
				ToolPath = GetCakeToolPath (),
				Arguments = new Dictionary<string, string> { 
					{ "target", target },
					{ "version", buildVersion },
				},
				Verbosity = Verbosity.Diagnostic
			};

			// Run the script from the subfolder
			CakeExecuteScript (project.BuildScript, cakeSettings);
		}
	}
});

RunTarget (TARGET);