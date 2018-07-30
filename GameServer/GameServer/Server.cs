using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameProtocol;
namespace GameServer
{
    class Server
    {
        private Socket server;
        private int maxClient = 10;
        private int port = 35353;

        private Stack<ClientToken> pools;

        public Server()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Start()
        {
            server.Listen(maxClient);

            pools = new Stack<ClientToken>(maxClient);
            for (int i = 0; i < maxClient; i++)
            {
                ClientToken clientToken = new ClientToken();
                pools.Push(clientToken);
            }

            server.BeginAccept(AsyncAccept, null);
        }

        private void AsyncAccept(IAsyncResult result)
        {
            try
            {
                Socket client = server.EndAccept(result);

                ClientToken clientToken = pools.Pop();
                clientToken.socket = client;

                BeginReceive(clientToken);
                server.BeginAccept(AsyncAccept, null);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        private void BeginReceive(ClientToken clientToken)
        {
            try
            {
                clientToken.socket.BeginReceive(clientToken.buffer, 0, clientToken.buffer.Length, SocketFlags.None, EndReceive, clientToken);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        private void EndReceive(IAsyncResult result)
        {
            try
            {
                ClientToken clientToken = result.AsyncState as ClientToken;
                int len = clientToken.socket.EndReceive(result);
                if (len > 0)
                {
                    Byte[] data = new Byte[len];

                    Buffer.BlockCopy(clientToken.buffer, 0, data, 0, len);
                    clientToken.Receive(data);
                    BeginReceive(clientToken);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }
}
