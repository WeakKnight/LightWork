using GameProtocol;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientManager : MonoBehaviour {

    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private Byte[] buffer;

    public string serverIP = "127.0.0.1";
    public int serverPort = 35353;

    public Action<Protocol> receiveCallBack;

    public void SendMessage(Protocol protocol)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                Byte[] serializedData = ProtocolHelper.ConvertProtocolToBytes(protocol);
                byte[] clientMessageAsByteArray = GameProtocol.Encoder.Encode(serializedData);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("发送数据协议，协议类型为" + protocol.GetType().ToString());
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("套接字异常:" + socketException.ToString());
        }
    }

    #region singleton
    public static ClientManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    void Start()
    {
        buffer = new Byte[1024];
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }

        catch (Exception e)
        {
            Debug.Log("客户端连接异常:" + e.ToString());
        }
    }

    private IEnumerator InvokeCallbackInMainThread(Protocol protocol)
    {
        receiveCallBack?.Invoke(protocol);
        yield return null;
    }

    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            socketConnection.Connect(ip);

            while (socketConnection.Connected)
            {
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        Byte[] incomingData = new Byte[length];
                        Array.Copy(buffer, 0, incomingData, 0, length);
                        Byte[] decodingData = GameProtocol.Encoder.Decode(incomingData);
                        Protocol protocol = ProtocolHelper.ConvertBytesToProtocol(decodingData);
                        if (protocol != null)
                        {
                            Debug.Log("收到协议类型" + protocol.GetType().ToString());
                            MainThreadDispatcher.Instance().Enqueue(InvokeCallbackInMainThread(protocol));
                        }
                        else
                        {
                            Debug.Log("收到协议反序列化失败");
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("套接字异常:" + socketException.ToString());
        }
    }
}
