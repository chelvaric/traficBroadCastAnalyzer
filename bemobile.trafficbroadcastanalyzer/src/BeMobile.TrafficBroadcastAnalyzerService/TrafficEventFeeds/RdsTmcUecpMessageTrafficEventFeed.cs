using BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents;
using System;
using System.Collections.Generic;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    [Serializable]
    public sealed class RdsTmcUecpMessageTrafficEventFeed
    {
        public RdsTmcUecpMessageTrafficEventMetadata Header { get; internal set; }
        public List<TmcTrafficEvent> TrafficEvents { get; internal set; }
    }
}
