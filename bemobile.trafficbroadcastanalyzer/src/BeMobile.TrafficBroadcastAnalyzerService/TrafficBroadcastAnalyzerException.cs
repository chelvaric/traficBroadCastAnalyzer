using System;

namespace BeMobile.TrafficBroadcastAnalyzerService
{
    [Serializable]
    public sealed class TrafficBroadcastAnalyzerException : Exception
    {
        internal TrafficBroadcastAnalyzerException(string message)
            : base(message)
        {
        }

        internal TrafficBroadcastAnalyzerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
