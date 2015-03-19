using System;
using System.Collections;
using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds;
using BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace BeMobile.TrafficBroadcastAnalyzerService
{
    public sealed class TrafficBroadcastAnalyzerService
    {
        private readonly List<ConfigurationEntry> configurationEntries;
        private readonly RdsTmcReferenceTrafficEventFeedProcessor rdsTmcReferenceFeedProcessor;
        public static RdsTmcGroupTrafficEventFeed HasXmlList { get; set; }
        public static RdsTmcGroupTrafficEventFeed NoReflList { get; set; }

        public static RdsTmcGroupTrafficEventFeed HasRdsList { get; set; }
        public static RdsTmcGroupTrafficEventFeed NoRdsReflList { get; set; }

        public static TrafficBroadcastAnalyzerService Service { get; set; }

        public static bool isCompatring { get; set; } 

        public TrafficBroadcastAnalyzerService(List<ConfigurationEntry> configurationEntries)
        {
            RdsTmcConfigurationEntry conf = (RdsTmcConfigurationEntry)configurationEntries[0];
            this.configurationEntries = configurationEntries;
          this.RdsTmcTrafficEventFeedProcessor = new RdsTmcTrafficEventFeedProcessor(conf);
            this.RdsTmcTrafficEventFeedProcessor.Start();
            Service = this;

        }

        internal RdsTmcTrafficEventFeedProcessor RdsTmcTrafficEventFeedProcessor { get; private set; }

        public void Start()
        {

        }

        public void Stop()
        {
             if(RdsTmcTrafficEventFeedProcessor!= null)
             this.RdsTmcTrafficEventFeedProcessor.Stop();

            if(rdsTmcReferenceFeedProcessor != null)
            this.rdsTmcReferenceFeedProcessor.Stop();
            
            
        }

        private void ProcessConfigEntries()
        {
            foreach (var configurationEntry in this.configurationEntries)
            {

                // Create and start the feed processors.

            }
        }

        private void OnRdsTmcReferenceTrafficEventCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           
        }

        public  void CompareXmlAndRds()
        {
            if (this.RdsTmcTrafficEventFeedProcessor.ReferenceFeedProcessor.TrafficEventFeed != null)
            {

                RdsTmcReferenceTrafficEventFeed Hold =
                    this.RdsTmcTrafficEventFeedProcessor.ReferenceFeedProcessor.TrafficEventFeed;
                if (Hold.TrafficEvents != null)
                {

                  
                    foreach (
                         var item in
                            this.RdsTmcTrafficEventFeedProcessor.GroupFeedProcessor.TrafficEventFeed.TrafficEvents)
                    {
                        Comparing newComp = new Comparing();
                        Comparing xmlComp = new Comparing();
                        newComp.WrongOrMissingEvents = new List<TmcEventCodeHistoryEntry>();
                        IEnumerable<TmcTrafficEvent> gevonden =
                            Hold.TrafficEvents.Where(x => x.LocationCode == item.LocationCode);

                        if (!gevonden.Any())
                        {
                            newComp.HasReference = false;
                        }
                        else
                        {
                            newComp.HasReference = true;
                            foreach (var gevondenItem in gevonden)
                            {
                                if (gevondenItem.EventCodeHistory.Count == item.EventCodeHistory.Count)
                                {
                                   
                                    for (int i = 0; i < gevondenItem.EventCodeHistory.Count; i++)
                                    {
                                        if (gevondenItem.EventCodeHistory[i].EventCode !=
                                            item.EventCodeHistory[i].EventCode ||
                                            item.EventCodeHistory[i].Extent != gevondenItem.EventCodeHistory[i].Extent)
                                        {
                                            newComp.HasWrongOrMissingEvents = true;
                                            newComp.WrongOrMissingEvents.Add(item.EventCodeHistory[i]);
                                        }
                                    }

                                    if (item.CodingDirection != gevondenItem.CodingDirection)
                                    {
                                        newComp.HasWrongDirectionCode = true;
                                    }
                                }

                            }
                        }

                        item.comparer = newComp;

                       
                    }

                   
                    
                        
                    

                }
            }
        }

        public void MakeComparedFeeds()
        {
                       TrafficBroadcastAnalyzerService.HasXmlList.TrafficEvents = new ObservableCollection<TmcTrafficEvent>();
                       TrafficBroadcastAnalyzerService.NoReflList.TrafficEvents = new ObservableCollection<TmcTrafficEvent>();
                       TrafficBroadcastAnalyzerService.NoRdsReflList.TrafficEvents = new ObservableCollection<TmcTrafficEvent>();
                       TrafficBroadcastAnalyzerService.HasRdsList.TrafficEvents = new ObservableCollection<TmcTrafficEvent>();

            foreach (var ev in this.RdsTmcTrafficEventFeedProcessor.GroupFeedProcessor.TrafficEventFeed.TrafficEvents)
            {
                if (ev.comparer.HasReference)
                {
                       HasXmlList.TrafficEvents.Add(ev);
                }
                else
                {
                      NoReflList.TrafficEvents.Add(ev);

                }
            }

            foreach (var tmcTrafficEvent in this.RdsTmcTrafficEventFeedProcessor.ReferenceFeedProcessor.TrafficEventFeed.TrafficEvents)
            {
                if (tmcTrafficEvent.comparer.HasWrongOrMissingEvents)
                {
                    
                }
                else
                {
                    
                }
            }
            

        }
    }
}
