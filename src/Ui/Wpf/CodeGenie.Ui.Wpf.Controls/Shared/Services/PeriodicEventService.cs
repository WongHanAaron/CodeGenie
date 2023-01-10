using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Shared.Services
{
    /// <summary> The service to periodically calling raising an event to do period actions </summary>
    public interface IPeriodicEventService
    {
        /// <summary> The millisecond period for the event to be raised </summary>
        public int MillisecondPeriod { get; set; }

        /// <summary> Event raised when period is elapsed </summary>
        public EventHandler<EventArgs> OnPeriodElapsed { get; set; }
    }

    public class PeriodicEventService : IPeriodicEventService
    {
        protected readonly ILogger<PeriodicEventService> Logger;
        protected Timer Timer;
        public PeriodicEventService(ILogger<PeriodicEventService> logger)
        {
            Logger = logger;
            SetPeriod(MillisecondPeriod);
        }

        private int _millisecondPeriod = 1000;
        public int MillisecondPeriod
        {
            get => _millisecondPeriod;
            set
            {
                SetPeriod(value);
                _millisecondPeriod = value;
            }
        }

        public EventHandler<EventArgs> OnPeriodElapsed { get; set; }

        protected void SetPeriod(int millisecond)
        {
            Timer = new Timer(o =>
            {
                try
                {
                    // Logger.LogTrace($"Invoking '{nameof(OnPeriodElapsed)}' for {MillisecondPeriod}ms period");
                    OnPeriodElapsed?.Invoke(this, new EventArgs());
                }
                catch (Exception ex)
                {
                    Logger.LogError($"An error occurred during the '{OnPeriodElapsed}' {ex}");
                }
            }, null, 0, millisecond);
        }
    }
}
