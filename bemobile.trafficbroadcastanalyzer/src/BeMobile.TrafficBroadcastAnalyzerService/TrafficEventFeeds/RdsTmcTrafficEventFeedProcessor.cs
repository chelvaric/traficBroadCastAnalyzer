using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using System;
using System.Threading.Tasks;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    [Serializable]
    internal sealed class RdsTmcTrafficEventFeedProcessor : IDisposable
    {
        internal RdsTmcTrafficEventFeedProcessor(RdsTmcConfigurationEntry configurationEntry)
        {
            this.ConfigurationEntry = configurationEntry;
            this.ReferenceFeedProcessor = new RdsTmcReferenceTrafficEventFeedProcessor(configurationEntry);
            this.GroupFeedProcessor = new RdsTmcGroupTrafficEventFeedProcessor(configurationEntry);
            this.UecpMessageFeedProcessor = new RdsTmcUecpMessageTrafficEventProcessor(configurationEntry);
        }

        internal bool IsRunning { get; set; }
        internal RdsTmcConfigurationEntry ConfigurationEntry { get; private set; }
        internal RdsTmcReferenceTrafficEventFeedProcessor ReferenceFeedProcessor { get; set; }
        internal RdsTmcGroupTrafficEventFeedProcessor GroupFeedProcessor { get; set; }
        internal RdsTmcUecpMessageTrafficEventProcessor UecpMessageFeedProcessor { get; set; }

        public void Start()
        {
            this.IsRunning = true;
            var task = new Task(() => this.ReferenceFeedProcessor.Start());
            task.Start();
            var taskGroup = new Task(() => GroupFeedProcessor.Start());
            taskGroup.Start();
        }

        public void Stop()
        {
            this.IsRunning = false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
