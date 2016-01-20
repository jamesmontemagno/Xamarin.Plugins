#addin "Cake.FileHelpers"

var TARGET = Argument ("target", Argument ("t", "NuGetPack"));

var version = Argument ("pkgversion", EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? "0.0.9999");

Task ("Build").Does (() =>
{

	const string sln = "./Media.sln";
    const string cfg = "Release";

	NuGetRestore (sln);

    if (!IsRunningOnWindows ())
        DotNetBuild (sln, c => c.Configuration = cfg);
    else
        MSBuild (sln, c => { 
            c.Configuration = cfg;
            c.MSBuildPlatform = MSBuildPlatform.x86;
        });
});

Task ("NuGetPack")
	.IsDependentOn ("Build")
	.Does (() =>
{
	NuGetPack ("./Media.Plugin.nuspec", new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./",
		BasePath = "./",
	});	
});


RunTarget (TARGET);
