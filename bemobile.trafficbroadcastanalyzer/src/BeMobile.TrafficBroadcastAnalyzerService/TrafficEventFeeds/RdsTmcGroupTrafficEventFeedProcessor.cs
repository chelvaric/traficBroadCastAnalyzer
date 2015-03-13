using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using System;
using System.Management;
using System.Text;
using System.Threading;
using System.Timers;
using BeMobile.Rds;
using BeMobile.Rds.Tmc;
using BeMobile.TrafficBroadcastAnalyzerService.RdsDecodeingAndStream;
using BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents;
using Timer = System.Timers.Timer;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    internal sealed class RdsTmcGroupTrafficEventFeedProcessor : IDisposable
    {
        private const int ProcessingTimerInterval = 1000;
        private readonly RdsTmcConfigurationEntry configurationEntry;
        private readonly Timer processingTimer;
        private bool isProcessing;
        private bool isDisposed;
        private Thread DecodingThread = new Thread( new ThreadStart(DecoderTest.StartDecoding));
        private MultiThreadSocketRdmListener _socketRead = new MultiThreadSocketRdmListener();
        private Thread SocketInRead;

        internal RdsTmcGroupTrafficEventFeedProcessor(RdsTmcConfigurationEntry configurationEntry)
        {
            this.processingTimer = new Timer();
            this.processingTimer.Interval = ProcessingTimerInterval;
            this.processingTimer.Elapsed += this.OnProcessingTimerElapsed;
            this.configurationEntry = configurationEntry;
            this.TrafficEventFeed = new RdsTmcReferenceTrafficEventFeed();
        }

        internal RdsTmcReferenceTrafficEventFeed TrafficEventFeed { get; private set; }

        public void Start()
        {
            if (SocketInRead == null)
            {
                SocketInRead = new Thread(new ThreadStart(_socketRead.ConnectListen));
                SocketInRead.Start();
            }

            if (DecodingThread.IsAlive != true)
            {
              DecodingThread.Start();
            }

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
            // TODO: process RDS TMC group traffic events.
            int lenghtofque = DecoderTest.DecodedGroupfeed.Count;
            for (int i=0; i < lenghtofque; i++)
            {
                RdsGroup temp;
                if (DecoderTest.DecodedGroupfeed.TryDequeue(out temp))
                {
                    if (temp.Metadata.Exception == null)
                    {
                        var b = checkGroup(temp);
                    }

                }

            }

        }

        private TmcTrafficEvent checkGroup(RdsGroup data)
        {
            TmcTrafficEvent temp = new TmcTrafficEvent();

            var datafields = data.DataFields as Group8ATmcEventSingleGroupDataFields;
            if (datafields != null)
            {
                if (datafields.CodingDirection.HasValue)
                {
                    Console.WriteLine(datafields.CodingDirection);

                }

            }

           var datafieldmulti = data.DataFields as Group8ATmcEventMultiGroupFirstDataFields;
            if (datafields != null)
            {
                Console.WriteLine(datafields.ToString());

            }

            var datafieldmultisec = data.DataFields as Group8ATmcEventMultiGroupSubseqDataFields;
            if(datafields != null)
            {
                Console.WriteLine(datafields.ToString());


            }
            return null;
        }
    }
}
