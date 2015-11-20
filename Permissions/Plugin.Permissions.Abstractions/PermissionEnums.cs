namespace Plugin.Permissions.Abstractions
{
    /// <summary>
    /// Status of a permission
    /// </summary>
    public enum PermissionStatus
    {
        /// <summary>
        /// Denied by user
        /// </summary>
        Denied,
        /// <summary>
        /// Feature is disabled on device
        /// </summary>
        Disabled,
        /// <summary>
        /// Granted by user
        /// </summary>
        Granted,
        /// <summary>
        /// Restricted (only iOS)
        /// </summary>
        Restricted,
        /// <summary>
        /// Permission is in an unknown state
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Permission group that can be requested
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// The unknown permission only used for return type, never requested
        /// </summary>
        Unknown,
        /// <summary>
        /// Android: Calendar
        /// iOS: Calendar (Events)
        /// </summary>
        Calendar,
        /// <summary>
        /// Android: Camera
        /// iOS: Photos (Camera Roll and Camera)
        /// </summary>
        Camera,
        /// <summary>
        /// Android: Contacts
        /// iOS: AddressBook
        /// </summary>
        Contacts,
        /// <summary>
        /// Android: Fine and Coarse Location
        /// iOS: CoreLocation (Always and WhenInUse)
        /// </summary>
        Location,
        /// <summary>
        /// Android: Microphone
        /// iOS: Microphone
        /// </summary>
        Microphone,
        /// <summary>
        /// Android: Phone
        /// iOS: Nothing
        /// </summary>
        Phone,
        /// <summary>
        /// Android: Nothing
        /// iOS: Photos
        /// </summary>
        Photos,
        /// <summary>
        /// Android: Nothing
        /// iOS: Reminders
        /// </summary>
        Reminders,
        /// <summary>
        /// Android: Body Sensors
        /// iOS: CoreMotion
        /// </summary>
        Sensors,
        /// <summary>
        /// Android: Sms
        /// iOS: Nothing
        /// </summary>
        Sms,
        /// <summary>
        /// Android: External Storage
        /// iOS: Nothing
        /// </summary>
        Storage
    }
}
