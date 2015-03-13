using System;

namespace BeMobile.TrafficBroadcastAnalyzerService.Configuration
{
    [Serializable]
    public class ConfigurationEntry
    {
        public string Name { get; set; }
        public string RepositoryUsername { get; set; }
        public string RepositoryPassword { get; set; }
        public string ReferenceTrafficEventsMapId { get; set; }
    }
}
