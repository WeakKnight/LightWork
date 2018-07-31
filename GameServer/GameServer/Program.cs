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
            Server server = new Server();
            server.Start();
            Thread.Sleep(-1);
        }

        static void SerailizingTestCase()
        {
            LoginProtocol loginProtocol = new LoginProtocol() { userId = "abc",userPassword = "xixixi" };
            byte[] haha;
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.SerializeWithLengthPrefix<LoginProtocol>(ms, loginProtocol, PrefixStyle.Base128);
                haha = new Byte[ms.Length];
                ms.Position = 0;
                ms.Read(haha, 0, haha.Length);
            }

            LoginProtocol result;

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(haha, 0, haha.Length);
                ms.Position = 0;
                result = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LoginProtocol>(ms, PrefixStyle.Base128);
            }
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
