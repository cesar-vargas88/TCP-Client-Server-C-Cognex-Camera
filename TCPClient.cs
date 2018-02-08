using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClockDetection
{
    class TCPClient
    {
        Thread ThreadClient;
        IPAddress IPAddress;
        int Port;
        int Delay;

        public TCPClient(string ipaddress, int port, int delay)
        {
            IPAddress = IPAddress.Parse(ipaddress);
            Port = port;
        }
        public void Start()
        {
            ThreadClient = new Thread(new ThreadStart(this.Writing));
            ThreadClient.IsBackground = true;
            ThreadClient.Start();
        }

        void Writing()
        {
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba;
                byte[] ReceivedDataBuffer = new byte[512];
                int BytesReceived;
                string data = "";
                String str  = "";
                Stream stm;

                TcpClient tcpClient = new TcpClient();
                Console.WriteLine("Connecting with server.....");
                tcpClient.Connect(IPAddress, Port);
                Console.WriteLine("Connected");
                stm = tcpClient.GetStream();

                while (true)
                {
                    BytesReceived = stm.Read(ReceivedDataBuffer, 0, 100);
                    data = Encoding.ASCII.GetString(ReceivedDataBuffer);
                    data = data.Substring(0, BytesReceived);
                    Console.Write(data);

                    if (data.Contains("User: "))
                    {
                        Thread.Sleep(1000);
                        ba = asen.GetBytes("admin\r\n");
                        stm.Write(ba, 0, ba.Length);
                        Console.WriteLine("admin");
                        BytesReceived = stm.Read(ReceivedDataBuffer, 0, 100);
                        data = Encoding.ASCII.GetString(ReceivedDataBuffer);
                        data = data.Substring(0, BytesReceived);
                        Console.Write(data);

                        if (data.Contains("Password: "))
                        {
                            Thread.Sleep(1000);
                            ba = asen.GetBytes("\r\n");
                            stm.Write(ba, 0, ba.Length);
                            Console.WriteLine("");

                            BytesReceived = stm.Read(ReceivedDataBuffer, 0, 100);
                            data = Encoding.ASCII.GetString(ReceivedDataBuffer);
                            data = data.Substring(0, BytesReceived);
                            Console.Write(data);

                            if (data.Contains("User Logged In"))
                            {
                                while (true)
                                {
                                    Thread.Sleep(2000);
                                    ba = asen.GetBytes("se8\r\n");
                                    stm.Write(ba, 0, ba.Length);
                                    Console.WriteLine("se8");

                                    BytesReceived = stm.Read(ReceivedDataBuffer, 0, 100);
                                    data = Encoding.ASCII.GetString(ReceivedDataBuffer);
                                    data = data.Substring(0, BytesReceived);
                                    Console.Write(data + Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
