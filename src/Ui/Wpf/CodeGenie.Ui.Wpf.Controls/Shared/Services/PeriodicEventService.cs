using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
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

        /// <summary> Register for and event to be raised when period is elapsed </summary>
        void Register(object requestor, int countsOfPeriod, Action<object, EventArgs> method);
    }

    public class PeriodicEventService : IPeriodicEventService
    {
        protected readonly ILogger<PeriodicEventService> Logger;
        protected ConcurrentQueue<PeriodicEventHandlerState> _periodicEventHandlingStates = new ConcurrentQueue<PeriodicEventHandlerState>();
        protected Timer Timer;

        public PeriodicEventService(ILogger<PeriodicEventService> logger)
        {
            Logger = logger;
            SetPeriod(MillisecondPeriod);
        }

        private int _millisecondPeriod = 500;
        public int MillisecondPeriod
        {
            get => _millisecondPeriod;
            set
            {
                SetPeriod(value);
                _millisecondPeriod = value;
            }
        }

        protected void SetPeriod(int millisecond)
        {
            Timer = new Timer(o =>
            {
                foreach (var periodicEventHandler in _periodicEventHandlingStates)
                {
                    try
                    {
                        periodicEventHandler.InvokeIfReady(o, new EventArgs());
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"An error occurred during event handling for '{periodicEventHandler?.Requestor?.GetType().Name}' {ex}");
                    }
                }
            }, null, 0, millisecond);
        }

        public void Register(object requestor, int countsOfPeriod, Action<object, EventArgs> method)
        {
            _periodicEventHandlingStates.Enqueue(new PeriodicEventHandlerState(requestor, countsOfPeriod, method));
        }

        protected class PeriodicEventHandlerState
        {
            public object Requestor { get; protected set; }
            public int CountsOfPeriodToRaiseEventOn { get; protected set; }
            public int CurrentPeriodCount { get; protected set; } = 0;
            public Action<object, EventArgs> MethodToInvoke { get; protected set; }

            public PeriodicEventHandlerState(object requestor, int countsOfPeriodToRaiseEventOn, Action<object, EventArgs> methodToInvoke)
            {
                Requestor = requestor;
                CountsOfPeriodToRaiseEventOn = countsOfPeriodToRaiseEventOn;
                MethodToInvoke = methodToInvoke;
            }

            public void InvokeIfReady(object sender, EventArgs args)
            {
                CurrentPeriodCount = (CurrentPeriodCount + 1 ) % CountsOfPeriodToRaiseEventOn;
                if (CurrentPeriodCount == 0)
                {
                    MethodToInvoke?.Invoke(sender, args);
                }
            }
        }
    }
}
