using System;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEventFeeds
{
    [Serializable]
    public sealed class RdsTmcGroupTrafficEventFeedMetadata
    {

    
       
        public bool? International { get; set; }
        public bool? National { get; set; }
        public bool? Regional { get; set; }
        public bool? Urban { get; set; }


        // location table 
        public byte? LocationTableNumber { get; set; }
        public byte? LocationTableCountryCode { get; set; }
        public byte? LocationTableExtendedCountryCode { get; set; }


        public override string ToString()
        {
            string temp =
               string.Format(  "Locationtable:  \n  location table number: {0}  \n   locationtableCountryCode: {1} \n ecc: {2}",
                LocationTableNumber,
                LocationTableCountryCode,
                LocationTableExtendedCountryCode
            );
            return temp;
        }
    }
}
