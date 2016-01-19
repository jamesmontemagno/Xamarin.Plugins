#addin "Cake.FileHelpers"

var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");

Task ("Settings").Does (() =>
{
	NuGetRestore ("./Settings/Refractored.XamPlugins.Settings.sln");

	DotNetBuild ("./Settings/Refractored.XamPlugins.Settings.sln", c => c.Configuration = "Release");
});

Task ("SettingsNuGetPack")
	.IsDependentOn ("Settings")
	.Does (() =>
{
	NuGetPack ("./Settings/Common/Xam.Plugins.Settings.nuspec", new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./",
		BasePath = "./",
	});	
});

