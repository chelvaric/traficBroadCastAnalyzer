using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeMobile.TrafficBroadcastAnalyzerService.RdsDecodeingAndStream
{
    class MultiThreadSocketRdmListener
    {
        private Socket _client;
        private Socket _clientRead;

       
        public static ConcurrentQueue<string> MessagQueue = new ConcurrentQueue<string>();

        public  void ConnectListen()
        {

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Rdm, (ProtocolType)113);
            
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse( "230.100.100.100"), 9999);
            _client.Bind(ep);

           
            _client.Listen(10);

            // start multithreading to be able to receive packets
          Thread  receiverThread = new Thread(new ThreadStart(PacketReceive));
            receiverThread.IsBackground = true;
            receiverThread.Start();

        

        
        }

        private void PacketReceive()
        {
            try
            {
                // enable socket for accepting connections
                _clientRead = _client.Accept();

                byte[] data = new byte[1024];
                string stringData;
                int recv;
                while (true)
                {
                    recv = _clientRead.Receive(data);
                    stringData = Encoding.ASCII.GetString(data, 0, recv);
                  //  Console.WriteLine(stringData);
                    MessagQueue.Enqueue(stringData);
                }
            }
            catch (Exception eb)
            {
                Console.WriteLine(eb.ToString());
            }
        }

    }
}
