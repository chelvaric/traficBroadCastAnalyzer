using System;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents
{
    [Serializable]
    public sealed class TmcEventCodeHistoryEntry
    {
        public DateTime Timestamp { get; internal set; }
        public byte Extent { get; internal set; }
        public int EventCode { get; internal set; }
    }
}
