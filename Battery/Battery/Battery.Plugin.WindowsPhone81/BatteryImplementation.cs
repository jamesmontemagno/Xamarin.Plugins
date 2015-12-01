using Plugin.Battery.Abstractions;
using System;
using Windows.ApplicationModel.Core;


namespace Plugin.Battery
{
    /// <summary>
    /// Implementation for Battery
    /// </summary>
    public class BatteryImplementation : BaseBatteryImplementation
    {
        private int last;
        private BatteryStatus status = BatteryStatus.Unknown;
        /// <summary>
        /// Default constructor
        /// </summary>
        public BatteryImplementation()
        {
            last = DefaultBattery.RemainingChargePercent;
            DefaultBattery.RemainingChargePercentChanged += RemainingChargePercentChanged;
        }

        async void RemainingChargePercentChanged(object sender, object e)
        {
            if (DefaultBattery.RemainingChargePercent == 100)
                status = BatteryStatus.Full;
            else if (last > DefaultBattery.RemainingChargePercent)
                status = BatteryStatus.Discharging;
            else if (last < DefaultBattery.RemainingChargePercent)
                status = BatteryStatus.Charging;
            else
                status = BatteryStatus.Unknown;

            last = DefaultBattery.RemainingChargePercent; ;

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            if (dispatcher != null)
            {
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    OnBatteryChanged(new BatteryChangedEventArgs
                    {
                        RemainingChargePercent = DefaultBattery.RemainingChargePercent,
                        IsLow = DefaultBattery.RemainingChargePercent <= 15,
                        PowerSource = PowerSource,
                        Status = Status
                    });
                });
            }
            else
            {
                OnBatteryChanged(new BatteryChangedEventArgs
                {
                    RemainingChargePercent = DefaultBattery.RemainingChargePercent,
                    IsLow = DefaultBattery.RemainingChargePercent <= 15,
                    PowerSource = PowerSource,
                    Status = Status
                });
            }

        }

#if WINDOWS_APP
        Windows.Devices.Power.Battery battery;
        private Windows.Phone.Devices.Power.Battery DefaultBattery
        {
            get { return battery ?? (battery = Windows.Phone.Devices.Power.Battery.GetDefault()); }
        }
#else
        Windows.Phone.Devices.Power.Battery battery;
        private Windows.Phone.Devices.Power.Battery DefaultBattery
        {
            get { return battery ?? (battery = Windows.Phone.Devices.Power.Battery.GetDefault()); }
        } 
#endif
        /// <summary>
        /// Gets current level of battery
        /// </summary>
        public override int RemainingChargePercent
        {
            get
            {
                return DefaultBattery.RemainingChargePercent;
            }
        }

        /// <summary>
        /// Get the current status of the battery
        /// </summary>
        public override BatteryStatus Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// Get the power source currently
        /// </summary>
        public override Abstractions.PowerSource PowerSource
        {
            get
            {
                if (status == BatteryStatus.Full || status == BatteryStatus.Charging)
                    return Abstractions.PowerSource.Ac;

                return Abstractions.PowerSource.Battery;
            }
        }


        private bool disposed = false;


        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DefaultBattery.RemainingChargePercentChanged -= RemainingChargePercentChanged;
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}