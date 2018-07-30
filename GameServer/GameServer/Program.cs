using System;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
            Thread.Sleep(-1);
        }
    }
}
