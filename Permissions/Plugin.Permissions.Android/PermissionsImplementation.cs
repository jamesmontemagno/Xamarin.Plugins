using Android;
using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Plugin.Permissions
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class PermissionsImplementation : Java.Lang.Object, IPermissions, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        public Activity CurrentActivity { get; set; }

        public static PermissionsImplementation Current
        {
            get {  return (PermissionsImplementation)CrossPermissions.Current; }
        }

        public Task<bool> ShouldShowRequestPermissionRationale(Permission permission)
        {
            if(CurrentActivity == null)
            {
                Debug.WriteLine("Unable to detect current CurrentActivity, please call PermissionsImplementation.Init with current Application from your MainActivity.");
                return Task.FromResult(false);
            }

             var names = GetManifestNames(permission);

            //if isn't an android specific group then go ahead and return false;
            if(names == null)
                return Task.FromResult(false);

            if(names.Count == 0)
                return Task.FromResult(false);

            foreach(var name in names)
            {
                if(ActivityCompat.ShouldShowRequestPermissionRationale(CurrentActivity, name))
                    return Task.FromResult(true); 
            }

            return Task.FromResult(false);
        
        }

        public Task<bool> CheckPermission(Permission permission)
        {
            if(CurrentActivity == null)
            {
                Debug.WriteLine("Unable to detect current Activity, please call PermissionsImplementation.Init with current Application from your MainActivity.");
                return Task.FromResult(false);
            }
            
            var names = GetManifestNames(permission);

            //if isn't an android specific group then go ahead and return true;
            if(names == null)
                return Task.FromResult(true);

            if(names.Count == 0)
                return Task.FromResult(false);

            foreach(var name in names)
            {
                if (ContextCompat.CheckSelfPermission(CurrentActivity, name) == Android.Content.PM.Permission.Denied)
                    return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        object locker = new object();
        TaskCompletionSource<Dictionary<Permission, bool>> tcs;
        Dictionary<Permission, bool> results;
        public async Task<Dictionary<Permission, bool>> RequestPermissions(IEnumerable<Permission> permissions)
        {
            if (tcs != null && !tcs.Task.IsCompleted)
            {
                tcs.SetCanceled();
                tcs = null;
            }
            lock (locker)
            {
                results = new Dictionary<Permission, bool>();
            }
            if(CurrentActivity == null)
            {
                Debug.WriteLine("Unable to detect current CurrentActivity, please call PermissionsImplementation.Init with current Application from your MainActivity.");
                return results;
            }
            var permissionsToRequest = new List<string>();
            foreach (var permission in permissions)
            {
                if (!await CheckPermission(permission))
                    permissionsToRequest.AddRange(GetManifestNames(permission));
                else
                {
                    lock (locker)
                    {
                        results.Add(permission, true);
                    }
                }
            }

            if (permissionsToRequest.Count == 0)
                return results;

            tcs = new TaskCompletionSource<Dictionary<Permission, bool>>();

            ActivityCompat.RequestPermissions(CurrentActivity, permissionsToRequest.ToArray(), PermissionCode);

            return await tcs.Task;
        }

        const int PermissionCode = 25;
        public void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode != PermissionCode)
                return;

            if (tcs == null)
                return;

            for (var i = 0; i < permissions.Length; i++)
            {
                if(tcs.Task.Status == TaskStatus.Canceled)
                    return;

                var permission = GetPermissionForManifestName(permissions[i]);
                if (permission == Permission.Unknown)
                    continue;

                lock (locker)
                {
                    if (!results.ContainsKey(permission))
                       results.Add(permission, grantResults[i] == Android.Content.PM.Permission.Granted);
                }
            }
            tcs.SetResult(results);
        }

        private static Permission GetPermissionForManifestName(string permission)
        {
            switch (permission)
            {
                case Manifest.Permission.ReadCalendar:
                case Manifest.Permission.WriteCalendar:
                    return Permission.Calendar;
                case Manifest.Permission.Camera:
                    return Permission.Camera;
                case Manifest.Permission.ReadContacts:
                case Manifest.Permission.WriteContacts:
                case Manifest.Permission.GetAccounts:
                    return Permission.Contacts;
                case Manifest.Permission.AccessCoarseLocation:
                case Manifest.Permission.AccessFineLocation:
                    return Permission.Location;
                case Manifest.Permission.RecordAudio:
                    return Permission.Microphone;
                case Manifest.Permission.ReadPhoneState:
                case Manifest.Permission.CallPhone:
                case Manifest.Permission.ReadCallLog:
                case Manifest.Permission.WriteCallLog:
                case Manifest.Permission.AddVoicemail:
                case Manifest.Permission.UseSip:
                case Manifest.Permission.ProcessOutgoingCalls:
                    return Permission.Phone;
                case Manifest.Permission.BodySensors:
                    return Permission.Sensors;
                case Manifest.Permission.SendSms:
                case Manifest.Permission.ReceiveSms:
                case Manifest.Permission.ReadSms:
                case Manifest.Permission.ReceiveWapPush:
                case Manifest.Permission.ReceiveMms:
                    return Permission.Sms;
                case Manifest.Permission.ReadExternalStorage:
                case Manifest.Permission.WriteExternalStorage:
                    return Permission.Storage;
            }

            return Permission.Unknown;
        }

        private List<string> GetManifestNames(Permission permission)
        {
            var permissionNames = new List<string>();
            switch(permission)
            {
                case Permission.Calendar:
                    {
                        if(HasPermissionInManifest(Manifest.Permission.ReadCalendar))
                            permissionNames.Add(Manifest.Permission.ReadCalendar);
                        if(HasPermissionInManifest(Manifest.Permission.WriteCalendar))
                            permissionNames.Add(Manifest.Permission.WriteCalendar);
                    }
                    break;
                case Permission.Camera:
                   {
                        if(HasPermissionInManifest(Manifest.Permission.Camera))
                            permissionNames.Add(Manifest.Permission.Camera);
                    }
                    break;
                case Permission.Contacts:
                    {
                        if(HasPermissionInManifest(Manifest.Permission.ReadContacts))
                            permissionNames.Add(Manifest.Permission.ReadContacts);
                        
                        if(HasPermissionInManifest(Manifest.Permission.WriteContacts))
                            permissionNames.Add(Manifest.Permission.WriteContacts);
                        
                        if(HasPermissionInManifest(Manifest.Permission.GetAccounts))
                            permissionNames.Add(Manifest.Permission.GetAccounts);
                    }
                    break;  
                case Permission.Location:
                    {
                        if(HasPermissionInManifest(Manifest.Permission.AccessCoarseLocation))
                            permissionNames.Add(Manifest.Permission.AccessCoarseLocation);

                        
                        if(HasPermissionInManifest(Manifest.Permission.AccessFineLocation))
                            permissionNames.Add(Manifest.Permission.AccessFineLocation);
                    }
                    break;
                case Permission.Microphone:
                    {
                        if(HasPermissionInManifest(Manifest.Permission.RecordAudio))
                            permissionNames.Add(Manifest.Permission.RecordAudio);

                    }
                    break;
                case Permission.Phone:
                    {
                        if(HasPermissionInManifest(Manifest.Permission.ReadPhoneState))
                            permissionNames.Add(Manifest.Permission.ReadPhoneState);
                        
                        if(HasPermissionInManifest(Manifest.Permission.CallPhone))
                            permissionNames.Add(Manifest.Permission.CallPhone);
                        
                        if(HasPermissionInManifest(Manifest.Permission.ReadCallLog))
                            permissionNames.Add(Manifest.Permission.ReadCallLog);
                        
                        if(HasPermissionInManifest(Manifest.Permission.WriteCallLog))
                            permissionNames.Add(Manifest.Permission.WriteCallLog);
                        
                        if(HasPermissionInManifest(Manifest.Permission.AddVoicemail))
                            permissionNames.Add(Manifest.Permission.AddVoicemail);
                        
                        if(HasPermissionInManifest(Manifest.Permission.UseSip))
                            permissionNames.Add(Manifest.Permission.UseSip);
                        
                        if(HasPermissionInManifest(Manifest.Permission.ProcessOutgoingCalls))
                            permissionNames.Add(Manifest.Permission.ProcessOutgoingCalls);
                    }
                    break;
                case Permission.Sensors:
                    {
                        if(HasPermissionInManifest(Manifest.Permission.BodySensors))
                            permissionNames.Add(Manifest.Permission.BodySensors);
                    }
                    break;
                case Permission.Sms:
                   {
                        if(HasPermissionInManifest(Manifest.Permission.SendSms))
                            permissionNames.Add(Manifest.Permission.SendSms);
                       
                        if(HasPermissionInManifest(Manifest.Permission.ReceiveSms))
                            permissionNames.Add(Manifest.Permission.ReceiveSms);
                       
                        if(HasPermissionInManifest(Manifest.Permission.ReadSms))
                            permissionNames.Add(Manifest.Permission.ReadSms);
                       
                        if(HasPermissionInManifest(Manifest.Permission.ReceiveWapPush))
                            permissionNames.Add(Manifest.Permission.ReceiveWapPush);
                       
                        if(HasPermissionInManifest(Manifest.Permission.ReceiveMms))
                            permissionNames.Add(Manifest.Permission.ReceiveMms);
                    }
                    break;
                case Permission.Storage:
                    {
                        if(HasPermissionInManifest(Manifest.Permission.ReadExternalStorage))
                            permissionNames.Add(Manifest.Permission.ReadExternalStorage);
                        
                        if(HasPermissionInManifest(Manifest.Permission.WriteExternalStorage))
                            permissionNames.Add(Manifest.Permission.WriteExternalStorage);
                    }
                    break;
                default:
                    return null;
            }

            return permissionNames;
        }

        IList<string> requestedPermissions = null;
        private bool HasPermissionInManifest(string permission)
        {
            try
            {
                if(requestedPermissions != null)
                    return requestedPermissions.Any(r => r.Equals(permission, StringComparison.InvariantCultureIgnoreCase));

                var info = CurrentActivity.PackageManager.GetPackageInfo(CurrentActivity.PackageName, Android.Content.PM.PackageInfoFlags.Permissions);
                requestedPermissions = info.RequestedPermissions;
                
                if(requestedPermissions == null)
                    return false;

                return requestedPermissions.Any(r => r.Equals(permission, StringComparison.InvariantCultureIgnoreCase));
            }
            catch(Exception ex)
            {
                Console.Write("Unable to check manifest for permission: " + ex.Message);
            }
            return false;
        }

    }
}