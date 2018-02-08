using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClockDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPServer CognexServer = new TCPServer("10.100.100.48", 1001);
            CognexServer.Start();
            Thread.Sleep(15000);
            TCPClient CognexClient = new TCPClient("10.100.100.30", 23, 1000);
            CognexClient.Start();

            while (true)
            { }
        }
    }
}
