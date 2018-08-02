using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameProtocol;
namespace GameServer
{
    public class Server
    {
        private Socket server;
        private int maxClient = 10;
        private int port = 35353;

        private Stack<ClientToken> pools;
        private List<Service> services = new List<Service>();
        public Dictionary<Type, Database> databases = new Dictionary<Type, Database>();

        public Server()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Start()
        {
            server.Listen(maxClient);

            //注册数据库
            RegisterDatabase();

            //注册所有服务
            RegisterServices();

            pools = new Stack<ClientToken>(maxClient);
            for (int i = 0; i < maxClient; i++)
            {
                ClientToken clientToken = new ClientToken();
                
                //给client注册进去所有的服务
                for (int j = 0; j < services.Count; j++)
                {
                    Service service = services[j];
                    clientToken.receiveCallBack += service.Execute;
                }

                pools.Push(clientToken);
            }

            server.BeginAccept(AsyncAccept, null);
        }

        private void RegisterDatabase()
        {
            //注册账号数据库
            databases[typeof(AccountDatabase)] = new AccountDatabase();
        }

        private void RegisterServices()
        {
            Debug.Log("已注册 登录服务");
            services.Add(new LoginService(this));
            Debug.Log("已注册 注册服务");
            services.Add(new RegisterService(this));
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
