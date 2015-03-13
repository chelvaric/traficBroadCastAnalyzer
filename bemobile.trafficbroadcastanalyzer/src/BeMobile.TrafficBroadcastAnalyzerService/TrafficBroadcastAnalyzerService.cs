using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace BeMobile.TrafficBroadcastAnalyzerService
{
    public sealed class TrafficBroadcastAnalyzerService
    {
        private readonly List<ConfigurationEntry> configurationEntries;
        private readonly RdsTmcReferenceTrafficEventFeedProcessor rdsTmcReferenceFeedProcessor;

        

        public TrafficBroadcastAnalyzerService(List<ConfigurationEntry> configurationEntries)
        {
            RdsTmcConfigurationEntry conf = (RdsTmcConfigurationEntry)configurationEntries[0];
            this.configurationEntries = configurationEntries;
          this.RdsTmcTrafficEventFeedProcessor = new RdsTmcTrafficEventFeedProcessor(conf);
            this.RdsTmcTrafficEventFeedProcessor.Start();

            
        }

        internal RdsTmcTrafficEventFeedProcessor RdsTmcTrafficEventFeedProcessor { get; private set; }

        public void Start()
        {

        }

        public void Stop()
        {
            
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
            // TODO
        }

        private void OnRdsTmcUecpTrafficEventCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TODO
        }
    }
}
