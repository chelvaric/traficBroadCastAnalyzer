using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using BeMobile.Parsers.TrafficEvents;
using BeMobile.Repository;
using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using System;
using System.Collections.Generic;
using System.Management;
using System.Timers;
using System.Xml;
using BeMobile.Rds.Tmc;
using BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    internal sealed class RdsTmcReferenceTrafficEventFeedProcessor : IDisposable
    {
        private const int processingTimerInterval = 10000;
        private readonly RdsTmcConfigurationEntry configurationEntry;
        private readonly RepositoryManager repositoryManager;
        private readonly Timer processingTimer;
        private bool isProcessing;
        private bool isDisposed;
        private bool isFirstXml = false;


        internal RdsTmcReferenceTrafficEventFeedProcessor(RdsTmcConfigurationEntry configurationEntry)
        {
            this.processingTimer = new Timer();
            this.processingTimer.Interval = processingTimerInterval;
            this.processingTimer.Elapsed += this.OnProcessingTimerElapsed;
            this.configurationEntry = configurationEntry;
            this.repositoryManager = new RepositoryManager(
                configurationEntry.RepositoryUsername,
                configurationEntry.RepositoryPassword);
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

        private async void OnProcessingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Prevent the timer from caling the same instance multiple times at the same time.
            if (this.isProcessing)
            {
                return;
            }
            this.isProcessing = true;
            await this.ProcessTrafficEvents();
            this.isProcessing = false;
        }

        private async Task ProcessTrafficEvents()
        {
           

            try
            {

                var result = await this.repositoryManager.SmartDownloadToString(
                    BeMobileFiles.TMCEventsXml,
                    this.configurationEntry.ReferenceTrafficEventsMapId,
                    DateTime.Now - new TimeSpan(0,0,1,0));
                if (result.NewData && result.Data != null)
                {

                    TrafficEventFeed.TrafficEvents = new List<TmcTrafficEvent>();

                    var trafficEventsXDocument = XDocument.Parse(result.Data);
                    var trafficEventsXmlParser = new TrafficEventsXmlParser();
                    var trafficEvents = trafficEventsXmlParser.ParseDocument(trafficEventsXDocument);

                    foreach (Global.DTO.TrafficEvent trafficEvent in trafficEvents)
                    {
                        TmcTrafficEvent temp = new TmcTrafficEvent();


                        // TmcPrimary is location code
                        temp.LocationCode = trafficEvent.TmcPrimary;
                        // coding direction

                        if (trafficEvent.TmcCodingDirection)
                        {
                            temp.CodingDirection = TmcCodingDirection.Positive;
                        }
                        else
                        {
                            temp.CodingDirection = TmcCodingDirection.Negative;
                        }


                        temp.EventCodeHistory = new List<TmcEventCodeHistoryEntry>();

                        // alertc event code
                        foreach (var alertc in trafficEvent.AlertC)
                        {
                            TmcEventCodeHistoryEntry entry = new TmcEventCodeHistoryEntry();
                            entry.EventCode = alertc.AlertC;
                            if (trafficEvent.TmcExtent.HasValue)
                            {
                                entry.Extent = trafficEvent.TmcExtent.Value;
                            }
                            temp.EventCodeHistory.Add(entry);


                        }

                        temp.Source = trafficEvent;

                        TrafficEventFeed.TrafficEvents.Add(temp);

                    }
                    Console.WriteLine(TrafficEventFeed.TrafficEvents.Count);
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }
        }
    }
}
