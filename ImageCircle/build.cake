#addin "Cake.FileHelpers"

var TARGET = Argument ("target", Argument ("t", "NuGetPack"));

var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");

Task ("Build").Does (() =>
{

	const string sln = "./ImageCircle.sln";
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
	NuGetPack ("./ImageCircle.Forms.Plugin.nuspec", new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./",
		BasePath = "./",
	});	
});


RunTarget (TARGET);
