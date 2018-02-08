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
    class TCPServer
    {
        Thread ThreadServer;
        IPAddress IPAddress;
        int Port;

        public TCPServer(string ipaddress, int port)
        {
            IPAddress = IPAddress.Parse(ipaddress);
            Port = port;
        }
        public void Start()
        {
            ThreadServer = new Thread(new ThreadStart(this.Listening));
            ThreadServer.IsBackground = true;
            ThreadServer.Start();
        }

        void Listening()
        {
            TcpListener myServer;          
            Socket      mySocket;

            // Initializes the server listener.
            myServer = new TcpListener(IPAddress, Port);
            // Start listening for client requests.
            myServer.Start();
            Console.WriteLine("The socket Endpoint is :" + myServer.LocalEndpoint);
            Console.WriteLine("Waiting for a connection.....");
            mySocket = myServer.AcceptSocket();
            Console.WriteLine("Connection accepted from " + mySocket.RemoteEndPoint);

            byte[] ReceivedDataBuffer = new byte[512];  // Create a buffer to receive data
            int BytesReceived = 0;                      // Stores the number of bytes received into BytesReceived
            String data = "";                           // Store all bytes received into data Satring and write them into the console
            ASCIIEncoding asen = new ASCIIEncoding();   // Creates an instance of ASCIIEncoding class

            while (true)
            {
                if (!mySocket.Connected)
                {
                    Console.WriteLine("Socket disconnected" + Environment.NewLine);
                    myServer.Start();
                    Console.WriteLine("The socket Endpoint is :" + myServer.LocalEndpoint);
                    Console.WriteLine("Waiting for a connection.....");
                    mySocket = myServer.AcceptSocket();
                    Console.WriteLine("Connection accepted from " + mySocket.RemoteEndPoint);
                }
                else
                {
                    string ParcialData = "";
                    do
                    {
                        Array.Clear(ReceivedDataBuffer, 0, ReceivedDataBuffer.Length);
                        //mySocket receives data from a bound socket into a DataReceiveBuffer and stores the number of bytes received into BytesReceived
                        BytesReceived = mySocket.Receive(ReceivedDataBuffer);

                        if (BytesReceived == 0)
                        {
                            mySocket.Close();
                        }

                        // Store all bytes received into data Satring and write them into the console
                        data = Encoding.ASCII.GetString(ReceivedDataBuffer);
                        ParcialData += data.Substring(0, BytesReceived);

                    } while (!ParcialData.Contains("\r") && !ParcialData.Contains("\\r"));

                    Console.WriteLine("Received: " + ParcialData);
                }
            }
        }
    }
}
