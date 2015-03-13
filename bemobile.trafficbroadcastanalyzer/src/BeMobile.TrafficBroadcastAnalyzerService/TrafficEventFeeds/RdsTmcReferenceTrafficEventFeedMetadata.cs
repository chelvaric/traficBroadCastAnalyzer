using System;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    [Serializable]
    public sealed class RdsTmcReferenceTrafficEventFeedMetadata
    {
        public string ServiceProviderName { get; internal set; }
        public byte LocationTableNumber { get; internal set; }
        public byte LocationTableCountryCode { get; internal set; }
        public DateTime LocationTableVersion { get; internal set; }
    }
}
