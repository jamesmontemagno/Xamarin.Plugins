using System.Reflection;
using System.Runtime.InteropServices;
using Android.App;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Plugin.Contacts")]
[assembly: AssemblyProduct("Plugin.Contacts")]
[assembly: ComVisible(false)]

[assembly: UsesPermission(Android.Manifest.Permission.ReadContacts)]