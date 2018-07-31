using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using GameProtocol;
namespace GameServer
{
    public class ClientToken
    {
        public Socket socket;
        public Byte[] buffer;

        private const int size = 1024;
        private List<Byte> receiveCache;
        private bool isReceiving;

        private Queue<Byte[]> sendCache;
        private bool isSending;

        public Action<LoginProtocol> receiveCallBack;

        public ClientToken()
        {
            buffer = new Byte[1024];
            receiveCache = new List<Byte>();
            sendCache = new Queue<Byte[]>();
        }

        public void Receive(Byte[] data)
        {
            Debug.Log("接收到数据");

            receiveCache.AddRange(data);
            if (!isReceiving)
            {
                isReceiving = true;
                ReadData();
            }
        }

        private void ReadData()
        {
            Byte[] data = GameProtocol.Encoder.Decode(ref receiveCache);

            if (data != null)
            {
                LoginProtocol loginProtocol = ProtocolHelper.ConvertBytesToProtocol<LoginProtocol>(data);
                Debug.Log("成功解析数据协议" + loginProtocol.userId + ":" + loginProtocol.userPassword);
                receiveCallBack?.Invoke(loginProtocol);
                ReadData();
            }
            else
            {
                isReceiving = false;
            }
        }

        public void Send()
        {
            try
            {
                if (sendCache.Count == 0)
                {
                    isSending = false;
                    return;
                }
                Byte[] data = sendCache.Dequeue();
                int count = data.Length / size;
                int len = size;

                for (int i = 0; i < count + 1; i++)
                {
                    if (i == count)
                    {
                        len = data.Length - i * size;
                    }
                    socket.Send(data, i * size, len, SocketFlags.None);
                }
                Debug.Log("发送成功");
                Send();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        public void WriteSendData(Byte[] data)
        {
            sendCache.Enqueue(data);
            if (!isSending)
            {
                isSending = true;
                Send();
            }
        }
    }
}
