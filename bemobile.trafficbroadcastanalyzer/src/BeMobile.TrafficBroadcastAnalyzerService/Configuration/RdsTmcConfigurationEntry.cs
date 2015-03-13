using System;

namespace BeMobile.TrafficBroadcastAnalyzerService.Configuration
{
    [Serializable]
    public class RdsTmcConfigurationEntry : ConfigurationEntry
    {
        public string UecpServiceHostname { get; set; }
        public int UecpServicePort { get; set; }

       
    }
}
