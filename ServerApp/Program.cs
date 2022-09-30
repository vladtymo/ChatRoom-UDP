using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    public class ChatServer
    {
        const short port = 4041;
        const string address = "10.0.1.4";

        TcpListener server = null;

        public ChatServer()
        {
            server = new TcpListener(new IPEndPoint(IPAddress.Parse(address), port));
        }

        public void Start()
        {
            server.Start();

            Console.WriteLine("Waiting for connection...");

            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("Connected!");

            NetworkStream ns = client.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            StreamReader sr = new StreamReader(ns);

            while (true)
            {
                string message = sr.ReadLine();
                Console.WriteLine($"Got: {message} at {DateTime.Now.ToShortTimeString()} from {client.Client.LocalEndPoint}");

                sw.WriteLine("Thanks!");
                sw.Flush();
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();
            server.Start();
        }
    }
}
