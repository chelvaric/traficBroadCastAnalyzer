using BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents;
using System;
using System.Collections.ObjectModel;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    [Serializable]
    public sealed class RdsTmcGroupTrafficEventFeed
    {
        public RdsTmcGroupTrafficEventFeedMetadata Metadata { get; internal set; }
        public ObservableCollection<TmcTrafficEvent> TrafficEvents { get; internal set; }
    }
}
