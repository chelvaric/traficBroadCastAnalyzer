using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Collections.Concurrent;
using System.Threading;
using System.Globalization;
using BeMobile.Rds;


namespace BeMobileTcpStreamCopy
{
    class DecoderTest
    {

        public static ConcurrentQueue<RdsGroup> DecodedGroupfeed = new ConcurrentQueue<RdsGroup>(); 
        public static void StartDecoding()
        {
            List<Packet> packets = new List<Packet>();
            RdsGroupDecoderConfiguration config = new RdsGroupDecoderConfiguration();
            config.DecodeMultiGroups = true;
            config.Group3AOdaThreshold = 20;
            config.TmcEncryption = false;
            RdsGroupDecoder decoder = new RdsGroupDecoder(config);
            
            while (true)
            {

                string message;
                if (Program.Qued != null)
                {
                   
                    if (Program.Qued.TryDequeue(out message))
                    {
                        string[] array = message.Split(';');
                        if (array.Length == 3)
                        {
                            Packet temp = new Packet();

                            if (!array[0].StartsWith("CONNECT"))
                            {
                                temp.Message = "0x" + array[0];
                            }

                            temp.MessageTimeSendDecode = array[1];
                            temp.MessageTimeSendServer = array[2];

                            var group = new RdsGroup(temp.Message);
                            decoder.Decode(group);
                            if (group.Metadata.IsDecoded)
                            {
                                DecodedGroupfeed.Enqueue(group);
                                Console.WriteLine(group.DataFields.ProgrammeId);
                            }


                            packets.Add(temp);
                        }
                    }



                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                   
                 
                  

                        
            }




        }

    }


    }
}
