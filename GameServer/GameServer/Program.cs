using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GameProtocol;
using ProtoBuf;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SerailizingTestCase();
            Server server = new Server();
            server.Start();
            Thread.Sleep(-1);
        }

        static void SerailizingTestCase()
        {
            LoginProtocol loginProtocol = new LoginProtocol() { userId = "abc",userPassword = "xixixi" };
            Byte[] bytes = ProtocolHelper.ConvertProtocolToBytes(loginProtocol);

            var result = ProtocolHelper.ConvertBytesToProtocol(bytes);

            if (result != null)
            {
                Debug.Log("序列化成功");
            }
            else
            {
                Debug.Log("序列化失败");
            }
        }
    }
}
