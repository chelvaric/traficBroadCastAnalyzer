using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace socketTestListener
{
    class MultiThreadSocketRDMListener
    {
        private Socket _client;
        private Socket _clientRead;

       
        public static ConcurrentQueue<string> MessagQueue = new ConcurrentQueue<string>();

        public  void connectListen()
        {

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Rdm, (ProtocolType)113);
            IPAddress adress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(adress, 0);
            
            _client.Listen(10);

            // start multithreading to be able to receive packets
          Thread  receiverThread = new Thread(new ThreadStart(packetReceive));
            receiverThread.IsBackground = true;
            receiverThread.Start();

        

        
        }

        private void packetReceive()
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
                    Console.WriteLine(stringData);
                    MessagQueue.Enqueue(stringData);
                }
            }
            catch (Exception eb)
            {
            }
        }

    }
}
