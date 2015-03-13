using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using System;
using System.Timers;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    public sealed class RdsTmcUecpMessageTrafficEventProcessor : IDisposable
    {
        private const int processingTimerInterval = 1000;
        private readonly RdsTmcConfigurationEntry configurationEntry;
        private readonly Timer processingTimer;
        private bool isProcessing;
        private bool isDisposed;

        internal RdsTmcUecpMessageTrafficEventProcessor(RdsTmcConfigurationEntry configurationEntry)
        {
            this.processingTimer = new Timer();
            this.processingTimer.Interval = processingTimerInterval;
            this.processingTimer.Elapsed += this.OnProcessingTimerElapsed;
            this.configurationEntry = configurationEntry;
            this.TrafficEventFeed = new RdsTmcReferenceTrafficEventFeed();
        }

        internal RdsTmcReferenceTrafficEventFeed TrafficEventFeed { get; private set; }

        public void Start()
        {
            if (this.processingTimer != null)
            {
                this.processingTimer.Start();
            }
        }

        public void Stop()
        {
            if (this.processingTimer != null)
            {
                this.processingTimer.Stop();
            }
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }
            if (this.processingTimer != null)
            {
                this.processingTimer.Dispose();
            }
            this.isDisposed = true;
        }

        private void OnProcessingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Prevent the timer from caling the same instance multiple times at the same time.
            if (this.isProcessing)
            {
                return;
            }
            this.isProcessing = true;
            this.ProcessTrafficEvents();
            this.isProcessing = false;
        }

        private void ProcessTrafficEvents()
        {
            // TODO: process RDS TMC UECP message traffic events.
        }
    }
}
