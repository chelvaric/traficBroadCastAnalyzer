using System;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    [Serializable]
    public class RdsTmcUecpMessageTrafficEventMetadata
    {
        public string ServiceProviderName { get; internal set; }
        public int LocationTableNumber { get; internal set; }
        public byte LocationTableCountryCode { get; internal set; }
        public byte LocationTableExtendedCountryCode { get; internal set; }
        public byte ServiceId { get; internal set; }
        public bool International { get; internal set; }
        public bool National { get; internal set; }
        public bool Regional { get; internal set; }
        public bool Urban { get; internal set; }
        public byte AlternativeFrequency1 { get; internal set; }
        public byte AlternativeFrequency2 { get; internal set; }
        public int AlternativeProgrammeId { get; internal set; }
    }
}
