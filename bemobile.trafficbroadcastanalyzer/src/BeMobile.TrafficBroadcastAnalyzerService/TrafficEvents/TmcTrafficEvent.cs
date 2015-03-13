using BeMobile.Rds.Tmc;
using System;
using System.Collections.Generic;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents
{
    [Serializable]
    public sealed class TmcTrafficEvent
    {
        public int LocationCode { get; internal set; }
        public TmcCodingDirection CodingDirection { get; internal set; }
        public List<TmcEventCodeHistoryEntry> EventCodeHistory { get; internal set; }
        public object Source { get; internal set; }
    }
}
