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

        public Action<Protocol, ClientToken> receiveCallBack;

        public ClientToken()
        {
            buffer = new Byte[1024];
            receiveCache = new List<Byte>();
            sendCache = new Queue<Byte[]>();
            receiveCallBack = BasicReceiveCallBack;
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

        private void BasicReceiveCallBack(Protocol protocol, ClientToken clientToken)
        {
            Debug.Log("收到协议,类型为:" + protocol.GetType().ToString() + ";");
        }

        private void ReadData()
        {
            Byte[] data = GameProtocol.Encoder.Decode(ref receiveCache);

            if (data != null)
            {
                Protocol protocol = ProtocolHelper.ConvertBytesToProtocol(data);
                receiveCallBack(protocol, this);
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
