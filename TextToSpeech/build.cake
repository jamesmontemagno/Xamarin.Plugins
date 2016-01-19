#addin "Cake.FileHelpers"

var TARGET = Argument ("target", Argument ("t", "Build"));

var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");

Task ("Build").Does (() =>
{

	const string sln = "./Refractored.Xam.TTS.sln";
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
	NuGetPack ("./Xam.Plugins.TextToSpeech.nuspec", new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./",
		BasePath = "./",
	});	
});


RunTarget (TARGET);