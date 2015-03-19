using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BeMobile.TrafficBroadcastAnalyzerService.TrafficEvents
{
   public sealed class Comparing
    {
       public Comparing()
       {
           HasReference = false;
           HasWrongDirectionCode = false;
           HasWrongDirectionCode = false;
       }

       public bool HasReference { get; set; }

        public bool HasWrongOrMissingEvents { get; set; }

        public List<TmcEventCodeHistoryEntry> WrongOrMissingEvents { get; set; }

        public bool HasWrongDirectionCode { get; set; }

    }
}
