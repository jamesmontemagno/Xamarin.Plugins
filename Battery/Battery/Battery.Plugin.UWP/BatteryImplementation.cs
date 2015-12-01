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
            DefaultBattery.ReportUpdated += RemainingChargePercentChanged;
        }

        async void RemainingChargePercentChanged(object sender, object e)
        {
            
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
           

            if (dispatcher != null)
            {
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    
                    OnBatteryChanged(new BatteryChangedEventArgs
                    {

                        RemainingChargePercent = RemainingChargePercent,
                        IsLow = RemainingChargePercent <= 15,
                        PowerSource = PowerSource,
                        Status = Status
                    });
                });
            }
            else
            {
                OnBatteryChanged(new BatteryChangedEventArgs
                {
                    RemainingChargePercent = RemainingChargePercent,
                    IsLow = RemainingChargePercent <= 15,
                    PowerSource = PowerSource,
                    Status = Status
                });
            }

        }
        
        private Windows.Devices.Power.Battery DefaultBattery
        {
            get { return Windows.Devices.Power.Battery.AggregateBattery; }
        }
        /// <summary>
        /// Gets current level of battery
        /// </summary>
        public override int RemainingChargePercent
        {
            get
            {
                var finalReport = DefaultBattery.GetReport();
                var finalPercent = -1;

                if (finalReport.RemainingCapacityInMilliwattHours.HasValue && finalReport.FullChargeCapacityInMilliwattHours.HasValue)
                {
                    finalPercent = (int)((finalReport.RemainingCapacityInMilliwattHours.Value /
                                     (double)finalReport.FullChargeCapacityInMilliwattHours.Value) * 100);
                }
                return finalPercent;
            }
        }

        /// <summary>
        /// Get the current status of the battery
        /// </summary>
        public override BatteryStatus Status
        {
            get
            {
                var report = DefaultBattery.GetReport();


                var percentage = RemainingChargePercent;

                if (percentage >= 1.0)
                    status = BatteryStatus.Full;
                else if (percentage < 0)
                    status = BatteryStatus.Unknown;
                else
                {
                    switch (report.Status)
                    {
                        case Windows.System.Power.BatteryStatus.Charging:
                            status = BatteryStatus.Charging;
                            break;
                        case Windows.System.Power.BatteryStatus.Discharging:
                            status = BatteryStatus.Discharging;
                            break;
                        case Windows.System.Power.BatteryStatus.Idle:
                            status = BatteryStatus.NotCharging;
                            break;
                        case Windows.System.Power.BatteryStatus.NotPresent:
                            status = BatteryStatus.Unknown;
                            break;
                    }
                }
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
                    DefaultBattery.ReportUpdated -= RemainingChargePercentChanged;
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}